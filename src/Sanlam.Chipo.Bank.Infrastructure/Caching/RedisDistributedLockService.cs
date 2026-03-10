using Microsoft.Extensions.Logging;
using Sanlam.Chipo.Bank.Domain.Caching;

namespace Sanlam.Chipo.Bank.Infrastructure.Caching;

internal class RedisDistributedLockService(
    ILogger<RedisDistributedLockService> logger) : IDistributedLockService
{
    public ValueTask<bool> GetSharedLockAsync(
        Guid sessionKey,
        string key, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Redis Distributed Shared Lock: {SessionKey}-{Key}", sessionKey, key);

        return ValueTask.FromResult(true);
    }

    public ValueTask<bool> GetSingleLockAsync(
        string key, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Redis Distributed Single Lock: {Key}", key);

        return ValueTask.FromResult(true);
    }

    public ValueTask<bool> ReleaseLockAsync(string key, CancellationToken cancellationToken)
    {
        logger.LogInformation("Releasing Redis Distributed Lock: {Key}", key);

        return ValueTask.FromResult(true);
    }

    public ValueTask<bool> ReleaseLockAsync(
        Guid sessionKey, 
        string key, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Releasing Redis Distributed Lock: {Key}", key);

        return ValueTask.FromResult(true);
    }
}