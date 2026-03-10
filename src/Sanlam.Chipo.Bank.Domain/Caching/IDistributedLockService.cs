namespace Sanlam.Chipo.Bank.Domain.Caching;

public interface IDistributedLockService
{
    /// <summary>Gets the single lock asynchronous.</summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    ValueTask<bool> GetSingleLockAsync(
        string key,
        CancellationToken cancellationToken);

    /// <summary>Gets the shared lock asynchronous.</summary>
    /// <param name="sessionKey">The session key.</param>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    ValueTask<bool> GetSharedLockAsync(
        Guid sessionKey,
        string key, 
        CancellationToken cancellationToken);

    /// <summary>Releases the lock asynchronous.</summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    ValueTask<bool> ReleaseLockAsync(
        string key,
        CancellationToken cancellationToken);

    /// <summary>Releases the lock asynchronous.</summary>
    /// <param name="sessionKey">The session key.</param>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    ValueTask<bool> ReleaseLockAsync(
        Guid sessionKey,
        string key,
        CancellationToken cancellationToken);
}