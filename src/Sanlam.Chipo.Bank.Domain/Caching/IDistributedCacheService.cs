namespace Sanlam.Chipo.Bank.Domain.Caching;

public interface IDistributedCacheService
{
    /// <summary>Gets the asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken);

    /// <summary>Gets the collection asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    Task<IReadOnlyList<T>?> GetCollectionAsync<T>(
        string key,
        CancellationToken cancellationToken);

    /// <summary>Sets the asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="expiration">The expiration.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    Task<T?> SetAsync<T>(
        string key,
        T? value,
        TimeSpan expiration,
        CancellationToken cancellationToken);

    /// <summary>Sets the asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="items">The items.</param>
    /// <param name="expiration">The expiration.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    Task<IEnumerable<T>> SetAsync<T>(
        string key,
        IEnumerable<T> items,
        TimeSpan expiration,
        CancellationToken cancellationToken);

    /// <summary>Sets the value asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="expiration">The expiration.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    Task<T> SetValueAsync<T>(
        string key,
        T value,
        TimeSpan expiration,
        CancellationToken cancellationToken);


    /// <summary>Removes the asynchronous.</summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    Task RemoveAsync(
        string key,
        CancellationToken cancellationToken);

    /// <summary>Removes the asynchronous.</summary>
    /// <param name="keyArray">The key array.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    Task RemoveAsync(
        string[] keyArray,
        CancellationToken cancellationToken);
}