using Sanlam.Chipo.Bank.Application.Models;
using Sanlam.Chipo.Bank.Domain.Result;

namespace Sanlam.Chipo.Bank.Application.Services.BankAccount;

public interface IBankingAccountService
{
    ValueTask<SystemActionResult<ModelAccountWithdraw>> WithdrawAsync(
        long accountNumber,
        decimal amount,
        bool releaseLock = true,
        CancellationToken cancellationToken = default);

    ValueTask<SystemActionResult<ModelAccountBalance>> GetBalanceAsync(
        long accountNumber,
        bool releaseLock = true,
        CancellationToken cancellationToken = default);

}