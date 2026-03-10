namespace Sanlam.Chipo.Bank.Domain.Caching;

public interface IDistributedLockService
{
    ValueTask<bool> GetSingleLockAsync(
        string key,
        CancellationToken cancellationToken);

    ValueTask<bool> GetSharedLockAsync(
        Guid sessionKey,
        string key, 
        CancellationToken cancellationToken);

    ValueTask<bool> ReleaseLockAsync(
        string key,
        CancellationToken cancellationToken);

    ValueTask<bool> ReleaseLockAsync(
        Guid sessionKey,
        string key,
        CancellationToken cancellationToken);
}