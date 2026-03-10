using Sanlam.Chipo.Bank.Application.Models;
using Sanlam.Chipo.Bank.Domain.Result;

namespace Sanlam.Chipo.Bank.Application.Services.BankAccount;

/// <summary>
///   A contract for the BankingAccount Service
/// </summary>
public interface IBankingAccountService
{
    /// <summary>Withdraws the asynchronous.</summary>
    /// <param name="accountNumber">The account number.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="releaseLock">if set to <c>true</c> [release lock].</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    ValueTask<SystemActionResult<ModelAccountWithdraw>> WithdrawAsync(
        long accountNumber,
        decimal amount,
        bool releaseLock = true,
        CancellationToken cancellationToken = default);

    /// <summary>Gets the balance asynchronous.</summary>
    /// <param name="accountNumber">The account number.</param>
    /// <param name="releaseLock">if set to <c>true</c> [release lock].</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    ValueTask<SystemActionResult<ModelAccountBalance>> GetBalanceAsync(
        long accountNumber,
        bool releaseLock = true,
        CancellationToken cancellationToken = default);

}