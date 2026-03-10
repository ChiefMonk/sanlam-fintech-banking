using Sanlam.Chipo.Bank.Application.Models;
using Sanlam.Chipo.Bank.Domain.Result;

namespace Sanlam.Chipo.Bank.Application.Repositories;

public interface IBankAccountRepository
{
    ValueTask<SystemActionResult<long>> CreateWithdrawOutboxAsync(
        string sessionKey,
        long accountNumber,
        decimal amount,
        CancellationToken cancellationToken);

    ValueTask<SystemActionResult<ModelBankAccount>> GetBalanceAsync(
        long accountNumber,
        CancellationToken cancellationToken);
}