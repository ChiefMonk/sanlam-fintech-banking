using Sanlam.Chipo.Bank.Application.Models;
using Sanlam.Chipo.Bank.Domain.Result;

namespace Sanlam.Chipo.Bank.Application.Repositories;

/// <summary>
///   A contract for the BankAccount DB Repository
/// </summary>
public interface IBankAccountRepository
{
    /// <summary>Creates the withdraw outbox asynchronous.</summary>
    /// <param name="sessionKey">The session key.</param>
    /// <param name="accountNumber">The account number.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    ValueTask<SystemActionResult<long>> CreateWithdrawOutboxAsync(
        string sessionKey,
        long accountNumber,
        decimal amount,
        CancellationToken cancellationToken);

    /// <summary>Gets the balance asynchronous.</summary>
    /// <param name="accountNumber">The account number.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    ValueTask<SystemActionResult<ModelBankAccount>> GetBalanceAsync(
        long accountNumber,
        CancellationToken cancellationToken);
}