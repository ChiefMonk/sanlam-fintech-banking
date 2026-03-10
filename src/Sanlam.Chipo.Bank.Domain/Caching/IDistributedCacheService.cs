namespace Sanlam.Chipo.Bank.Domain.Caching;

public interface IDistributedCacheService
{
    Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<T>?> GetCollectionAsync<T>(
        string key,
        CancellationToken cancellationToken);

    Task<T?> SetAsync<T>(
        string key,
        T? value,
        TimeSpan expiration,
        CancellationToken cancellationToken);

    Task<IEnumerable<T>> SetAsync<T>(
        string key,
        IEnumerable<T> items,
        TimeSpan expiration,
        CancellationToken cancellationToken);

    Task<T> SetValueAsync<T>(
        string key,
        T value,
        TimeSpan expiration,
        CancellationToken cancellationToken);


    Task RemoveAsync(
        string key,
        CancellationToken cancellationToken);

    Task RemoveAsync(
        string[] keyArray,
        CancellationToken cancellationToken);
}