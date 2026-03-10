using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Polly.Wrap;
using Sanlam.Chipo.Bank.Domain.Caching;
using Sanlam.Chipo.Bank.Domain.Options;
using StackExchange.Redis;

namespace Sanlam.Chipo.Bank.Infrastructure.Caching;

internal class RedisDistributedCacheService : IDistributedCacheService
{
	private const int CancelAfterSeconds = 2;
	private const int MinDurationOfBreakInMilliSeconds = 30_000;
	private const int ScanBatchSize = 500; // tune per workload
	private const int DeleteBatchSize = 1000;

	private readonly ILogger<RedisDistributedCacheService> _logger;
	private readonly IDistributedCache _cache;
	private readonly IConnectionMultiplexer _connection;
	private readonly IDatabase _database;
	private readonly AsyncPolicyWrap _policyWrap;
	private readonly JsonSerializerContext? _serializerContext;
	private static readonly ConcurrentDictionary<Type, string> TypeNameCache = new();

	public RedisDistributedCacheService(
		ILogger<RedisDistributedCacheService> logger,
		IOptions<RedisSettingOptions> redisOptions,
		IDistributedCache cache,
		IConnectionMultiplexer connection,
		JsonSerializerContext? serializerContext = null)
	{
		_logger = logger;
		_cache = cache;
		_connection = connection;
		_database = connection.GetDatabase();
		_serializerContext = serializerContext;

		var redisSettings = redisOptions.Value;

		// Fail fast first, then selectively retry, then trip the breaker.
		var timeoutPolicy = Policy.TimeoutAsync(
			seconds: CancelAfterSeconds,
			TimeoutStrategy.Pessimistic,
			onTimeoutAsync: (ctx, ts, _, ex) =>
			{
				logger.LogWarning(ex, "REDIS: Timeout after {Seconds}s. Operation={Operation}", ts.TotalSeconds, ctx.OperationKey);
				return Task.CompletedTask;
			});

		var retryPolicy = Policy
			.Handle<RedisException>() // only retry transient redis errors
			.WaitAndRetryAsync(
				retryCount: redisSettings.Options?.RetryCount ?? 3,
				sleepDurationProvider: _ => TimeSpan.FromMilliseconds(redisSettings.Options?.SleepInMs ?? 100),
				onRetry: (exception, timeSpan, retryCount, context) =>
				{
					logger.LogWarning(
						exception,
						"REDIS: Retry={RetryCount} in {SleepMs}ms, Operation={Operation}",
						retryCount, (int)timeSpan.TotalMilliseconds, context.OperationKey);
				});

		var circuitBreakerPolicy = Policy
			.Handle<RedisException>()
			.CircuitBreakerAsync(
				exceptionsAllowedBeforeBreaking: redisSettings.Options?.ExceptionsBeforeBreaking ?? 5,
				durationOfBreak: TimeSpan.FromMilliseconds(Math.Max(redisSettings.Options?.BreakInMs ?? 10000, MinDurationOfBreakInMilliSeconds)),
				onBreak: (ex, breakDelay) =>
				{
					logger.LogError(ex, "REDIS-CB: OPEN for {TotalSeconds}s", breakDelay.TotalSeconds);
				},
				onReset: () =>
				{
					logger.LogDebug("REDIS-CB: CLOSED (recovered).");
				},
				onHalfOpen: () =>
				{
					logger.LogDebug("REDIS-CB: HALF-OPEN. Next call is a trial.");
				});

		// Order matters: timeout (outer) -> retry -> breaker (inner)
		_policyWrap = Policy.WrapAsync(timeoutPolicy, retryPolicy, circuitBreakerPolicy);
	}



	public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			return default;
		}

		return await ExecuteSafely(
			operationName: $"GET:{key}",
			func: async ct =>
			{
				var bytes = await _cache.GetAsync(key, ct).ConfigureAwait(false);

				if (bytes is null || bytes.Length == 0)
				{
					_logger.LogDebug("REDIS-MISS {Key} {Type}", key, TypeName<T>());
					return default;
				}

				_logger.LogDebug("REDIS-HIT {Key} {Type} ({Bytes}B)", key, TypeName<T>(), bytes.Length);
				return Deserialize<T?>(bytes);
			},
			cancellationToken);
	}

	public async Task<IReadOnlyList<T>?> GetCollectionAsync<T>(string key, CancellationToken cancellationToken)
	{
		var cached = await GetAsync<IReadOnlyList<T>>(key, cancellationToken).ConfigureAwait(false);
		return (cached is { Count: > 0 }) ? cached : null;
	}

	public async Task<T?> SetAsync<T>(string key, T? value, TimeSpan expiration, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(key) || value is null || Equals(value, default(T)))
		{
			return value;
		}

		await ExecuteSafely(
			operationName: $"SET:{key}",
			func: async ct =>
			{
				var bytes = Serialize(value);
				await _cache.SetAsync(key, bytes, CreateCacheEntryOptions(expiration), ct).ConfigureAwait(false);
				_logger.LogDebug("REDIS-SET {Key} {Type} ({Bytes}B) exp={Exp}", key, TypeName<T>(), bytes.Length, expiration);
				return true;
			},
			cancellationToken);

		return value;
	}

	public async Task<IEnumerable<T>> SetAsync<T>(string key, IEnumerable<T> items, TimeSpan expiration, CancellationToken cancellationToken)
	{
		var arr = items as T[] ?? items.ToArray();
		
		if (arr.Length == 0)
		{
			return arr;
		}

		await SetAsync(key, arr, expiration, cancellationToken).ConfigureAwait(false);
		return arr;
	}

	public async Task<T> SetValueAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken)
	{
		await SetAsync(key, value, expiration, cancellationToken).ConfigureAwait(false);
		return value;
	}

	public async Task RemoveAsync(string key, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			return;
		}

		await ExecuteSafely(
			operationName: $"DEL:{key}",
			func: async ct =>
			{
				await _cache.RemoveAsync(key, ct).ConfigureAwait(false);
				_logger.LogDebug("REDIS-REM {Key}", key);

				return true;
			},
			cancellationToken);
	}

	public async Task RemoveAsync(string[] keyArray, CancellationToken cancellationToken)
	{
		if (keyArray.Length == 0)
		{
			return;
		}
		// fan-out deletes

		await Task.WhenAll(keyArray.Select(k => RemoveAsync(k, cancellationToken))).ConfigureAwait(false);
	}

	/// <summary>
	/// Deletes keys that contain either pattern (pattern1 OR pattern2) using SCAN and batched deletes.
	/// </summary>
	public async Task<long> RemoveAllAsync(string pattern1, string pattern2)
	{
		if (string.IsNullOrWhiteSpace(pattern1) && string.IsNullOrWhiteSpace(pattern2))
		{
			return 0;
		}

		var totalDeleted = 0L;

		try
		{
			_logger.LogInformation("REDIS: Starting pattern delete: '{P1}' OR '{P2}'", pattern1, pattern2);

			foreach (var endpoint in _connection.GetEndPoints())
			{
				var server = _connection.GetServer(endpoint);
				if (server is not { IsConnected: true, IsReplica: false })
				{
					continue;
				}

				var buffer = new List<RedisKey>(DeleteBatchSize);

				async Task FlushAsync()
				{
					if (buffer.Count == 0) return;
					var deleted = await _database.KeyDeleteAsync(buffer.ToArray()).ConfigureAwait(false);
					totalDeleted += deleted;
					buffer.Clear();
				}

				async IAsyncEnumerable<RedisKey> EnumeratePattern(string pat)
				{
					if (string.IsNullOrWhiteSpace(pat))
					{
						yield break;
					}
					await foreach (var key in server.KeysAsync(pattern: $"*{pat}*", pageSize: ScanBatchSize)) 
					{
						yield return key;
					}
				}

				async Task AddAndFlushAsync(RedisKey key)
				{
					buffer.Add(key);
					if (buffer.Count >= DeleteBatchSize)
						await FlushAsync().ConfigureAwait(false);
				}

				await foreach (var key in EnumeratePattern(pattern1))
				{
					await AddAndFlushAsync(key);
				}

				await foreach (var key in EnumeratePattern(pattern2))
				{
					await AddAndFlushAsync(key);
				}

				await FlushAsync().ConfigureAwait(false);
			}

			_logger.LogInformation("REDIS: Deleted {Total} keys", totalDeleted);

			return totalDeleted;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "REDIS: Error during RemoveAllAsync for '{P1}'/'{P2}'", pattern1, pattern2);
			return 0;
		}
	}
	private async Task<TOut?> ExecuteSafely<TOut>(
		string operationName, Func<CancellationToken, Task<TOut?>> func, 
		CancellationToken ct)
	{
		try
		{
			var ctx = new Context(operationName);
			return await _policyWrap.ExecuteAsync((_, token) => func(token), ctx, ct).ConfigureAwait(false);
		}
		catch (BrokenCircuitException ex)
		{
			_logger.LogError(ex, "REDIS-CB: OPEN during {Operation}", operationName);
		}
		catch (TimeoutRejectedException ex)
		{
			_logger.LogWarning(ex, "REDIS: Timeout during {Operation}", operationName);
		}
		catch (OperationCanceledException)
		{
			_logger.LogWarning("REDIS: Canceled {Operation}", operationName);
		}
		catch (RedisException ex)
		{
			_logger.LogError(ex, "REDIS: RedisException during {Operation}", operationName);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "REDIS: Unexpected error during {Operation}", operationName);
		}
		return default;
	}

	
	private static string TypeName<T>() => TypeNameCache.GetOrAdd(typeof(T), static t =>
	{
		if (typeof(IEnumerable).IsAssignableFrom(t) &&
			t != typeof(string))
		{
			return "List<T>";
		}
		if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
		{
			return Nullable.GetUnderlyingType(t)!.Name;
		}
		return t.Name;
	});

	private static DistributedCacheEntryOptions CreateCacheEntryOptions(TimeSpan expiration, TimeSpan? sliding = null)
	{
		// Choose one strategy in production. Keeping your hybrid logic by default:
		var slidingExp = sliding ?? TimeSpan.FromMinutes(Math.Min(expiration.TotalMinutes / 2, TimeSpan.FromHours(3).TotalMinutes));

		return new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = expiration,
			SlidingExpiration = slidingExp
		};
	}

	private TTarget? Deserialize<TTarget>(byte[]? bytes)
	{
		if (bytes is null || bytes.Length == 0) return default;

		if (_serializerContext is null)
		{
			return JsonSerializer.Deserialize<TTarget>(bytes, SerializerOptions);
		}

		// If using source-gen, try to resolve the JsonTypeInfo<TTarget>.
		if (_serializerContext.GetType().GetProperty(nameof(JsonSerializerContext.Options)) is not null)
		{
			// STJ doesn't expose a generic getter; if you have a generated context with strongly-typed properties,
			// prefer using those. For brevity, fallback to non-source-gen path when type info isn't easily accessible.
			return JsonSerializer.Deserialize<TTarget>(bytes, SerializerOptions);
		}

		return JsonSerializer.Deserialize<TTarget>(bytes, SerializerOptions);
	}

	private byte[] Serialize<TValue>(TValue value)
	{
		if (_serializerContext is null)
		{
			return JsonSerializer.SerializeToUtf8Bytes(value, SerializerOptions);
		}
		// As above: if you have generated JsonTypeInfo<TValue>, prefer that overload.
		return JsonSerializer.SerializeToUtf8Bytes(value, SerializerOptions);
	}

	private static readonly JsonSerializerOptions SerializerOptions = new()
	{
		PropertyNamingPolicy = null,
		WriteIndented = false,
		AllowTrailingCommas = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
	};
}