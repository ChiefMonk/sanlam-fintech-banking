using Microsoft.Extensions.Options;
using Sanlam.Chipo.Bank.Application.Messages;
using Sanlam.Chipo.Bank.Application.Models;
using Sanlam.Chipo.Bank.Application.Repositories;
using Sanlam.Chipo.Bank.Domain.Caching;
using Sanlam.Chipo.Bank.Domain.Enums;
using Sanlam.Chipo.Bank.Domain.Messaging;
using Sanlam.Chipo.Bank.Domain.Options;
using Sanlam.Chipo.Bank.Domain.Result;
using Sanlam.Chipo.Bank.Domain.Session;

namespace Sanlam.Chipo.Bank.Application.Services.BankAccount;

internal sealed class BankingAccountService(
    IUserSessionContext sessionContext,
    IDistributedLockService distributedLockService,
    IRabbitMqPublisherService rabbitMqPublisherService,
    IOptions<RabbitMqSettingOptions> rabbitOptions,
    IKafkaPublisherService kafkaPublisherService,
    IOptions<KafkaSettingOptions> kafkaOptions,
    IBankAccountRepository bankAccountRepository) : IBankingAccountService
{

    public async ValueTask<SystemActionResult<ModelAccountWithdraw>> WithdrawAsync(
        long accountNumber,
        decimal amount,
        bool releaseLock = true,
        CancellationToken cancellationToken = default)
    {
        var accountKey = CreateAccountLockKey(accountNumber);

        var lockAccount = await distributedLockService.GetSharedLockAsync(
            sessionContext.SessionKey,
            accountKey,
            cancellationToken);

        if (!lockAccount)
        {
            var errorResult = new SystemActionError(
                400,
                BankAccountActionResult.NotAvailable,
                $"The account is not available at the moment: {accountNumber}");

            return await ProcessResponseAsync(
                accountNumber,
                amount,
                errorResult,
                cancellationToken);
        }

        try
        {
            var bankAccountResult = await GetBalanceAsync(
                accountNumber,
                false,
                cancellationToken);

            if (bankAccountResult.IsFailure)
            {
                return await ProcessResponseAsync(
                    accountNumber,
                    amount,
                    bankAccountResult.Error,
                    cancellationToken);
            }

            var bankAccount = bankAccountResult.Value;

            if (bankAccount.BalanceAmount < amount)
            {
                var errorResult = new SystemActionError(
                    403,
                    BankAccountActionResult.InsufficientFunds,
                    $"The account has insufficient funds: {accountNumber}");

                return await ProcessResponseAsync(
                    accountNumber,
                    amount,
                    errorResult,
                    cancellationToken);
            }

            if (SomeWithdrawValidation(bankAccount, amount))
            {
                var errorResult = new SystemActionError(
                    403,
                    BankAccountActionResult.WithdrawalFailed,
                    $"The account withdraw failed: {accountNumber}");

                return await ProcessResponseAsync(
                    accountNumber,
                    amount,
                    errorResult,
                    cancellationToken);
            }

            var createOutboxWithdraw = await bankAccountRepository.CreateWithdrawOutboxAsync(
                sessionContext.SessionKey.ToString(),
                accountNumber,
                amount,
                cancellationToken);

            if (createOutboxWithdraw.IsFailure)
            {
                return await ProcessResponseAsync(
                    accountNumber,
                    amount,
                    createOutboxWithdraw.Error,
                    cancellationToken);
            }

            var model = new ModelAccountWithdraw()
            {
                AccountNumber = accountNumber,
                TimeUct = DateTime.UtcNow,
                WithdrawAmount = amount,
                BalanceAmount = bankAccount.BalanceAmount - amount
            };

            return await ProcessResponseAsync(
                accountNumber,
                amount,
                model,
                cancellationToken);
        }
        finally
        {
            if (releaseLock)
            {
                await distributedLockService.ReleaseLockAsync(
                    accountKey,
                    cancellationToken);
            }
        }

    }

    public async ValueTask<SystemActionResult<ModelAccountBalance>> GetBalanceAsync(
        long accountNumber,
        bool releaseLock = true,
        CancellationToken cancellationToken=  default)
    {
        var accountKey = CreateAccountLockKey(accountNumber);

        var lockAccount = await distributedLockService.GetSharedLockAsync(
                sessionContext.SessionKey,
                accountKey, 
                cancellationToken);

        if (!lockAccount)
        {
           var errorResult = new SystemActionError(
                400, 
                BankAccountActionResult.NotAvailable,
                $"The account is not available at the moment: {accountNumber}");

           return SystemActionResult.Failure<ModelAccountBalance>(errorResult);
        }

        try
        {
            var bankAccountResult = await bankAccountRepository
                .GetBalanceAsync(accountNumber, cancellationToken);

            if (bankAccountResult.IsFailure)
            {
                return SystemActionResult.Failure<ModelAccountBalance>(bankAccountResult.Error);
            }

            var bankAccount = bankAccountResult.Value;

            return SystemActionResult.Success(new ModelAccountBalance
            {
                AccountNumber = bankAccount.AccountNumber,
                TimeUct = DateTime.UtcNow,
                BalanceAmount = bankAccount.CurrentBalance
            });
        }
        finally
        {
            if (releaseLock)
            {
                await distributedLockService.ReleaseLockAsync(
                    accountKey,
                    cancellationToken);
            }
        }
    }

    private async Task<SystemActionResult<ModelAccountWithdraw>> ProcessResponseAsync(
        long accountNumber,
        decimal amount,
        SystemActionError error,
        CancellationToken cancellationToken)
    {
        var message = new ModelAccountWithdrawEvent(
            accountNumber,
            amount,
            DateTime.UtcNow,
            error.ResultCode,
            error.ResultDescription);

        await rabbitMqPublisherService.PublishMessage(
            rabbitOptions.Value.ExchangeWithdrawalEvents,
            rabbitOptions.Value.RoutingKeyWithdrawalEvents,
            message,
            cancellationToken);

        await kafkaPublisherService.PublishMessage(
            kafkaOptions.Value.TopicWithdrawalEvents,
            message,
            cancellationToken);

        return SystemActionResult.Failure<ModelAccountWithdraw>(error);
    }

    private async Task<SystemActionResult<ModelAccountWithdraw>> ProcessResponseAsync(
        long accountNumber,
        decimal amount,
        ModelAccountWithdraw withdraw,
        CancellationToken cancellationToken)
    {
        var message = new ModelAccountWithdrawEvent(
            accountNumber,
            amount,
            DateTime.UtcNow,
            BankAccountActionResult.Successful,
            $"The account withdraw successful: {accountNumber}");

        await rabbitMqPublisherService.PublishMessage(
            rabbitOptions.Value.ExchangeWithdrawalEvents,
            rabbitOptions.Value.RoutingKeyWithdrawalEvents,
            message,
            cancellationToken);

        return SystemActionResult.Success(withdraw);
    }

    private bool SomeWithdrawValidation(ModelAccountBalance balance, decimal amount)
    {
        return true;
    }

    private static string CreateAccountLockKey(long accountNumber)
    {
       return $"lock-{accountNumber}";
    }
}