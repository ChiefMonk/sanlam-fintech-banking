using Microsoft.Extensions.Logging;
using Sanlam.Chipo.Bank.Domain.Caching;

namespace Sanlam.Chipo.Bank.Infrastructure.Caching;

/// <summary>
///  Implementation for IDistributedCacheService
/// </summary>
internal class RedisDistributedLockService(
    ILogger<RedisDistributedLockService> logger) : IDistributedLockService
{
    /// <summary>Gets the shared lock asynchronous.</summary>
    /// <param name="sessionKey">The session key.</param>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public ValueTask<bool> GetSharedLockAsync(
        Guid sessionKey,
        string key, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Redis Distributed Shared Lock: {SessionKey}-{Key}", sessionKey, key);

        return ValueTask.FromResult(true);
    }

    /// <summary>Gets the single lock asynchronous.</summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public ValueTask<bool> GetSingleLockAsync(
        string key, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Redis Distributed Single Lock: {Key}", key);

        return ValueTask.FromResult(true);
    }

    /// <summary>Releases the lock asynchronous.</summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public ValueTask<bool> ReleaseLockAsync(string key, CancellationToken cancellationToken)
    {
        logger.LogInformation("Releasing Redis Distributed Lock: {Key}", key);

        return ValueTask.FromResult(true);
    }

    /// <summary>Releases the lock asynchronous.</summary>
    /// <param name="sessionKey">The session key.</param>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public ValueTask<bool> ReleaseLockAsync(
        Guid sessionKey, 
        string key, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Releasing Redis Distributed Lock: {Key}", key);

        return ValueTask.FromResult(true);
    }
}