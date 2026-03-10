using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Sanlam.Chipo.Bank.Application.Models;
using Sanlam.Chipo.Bank.Application.Repositories;
using Sanlam.Chipo.Bank.Domain.Caching;
using Sanlam.Chipo.Bank.Domain.Enums;
using Sanlam.Chipo.Bank.Domain.Result;
using Sanlam.Chipo.Bank.Infrastructure.Sql.Connection;
using System.Data;
using Microsoft.Extensions.Options;
using Sanlam.Chipo.Bank.Application.Messages;
using Sanlam.Chipo.Bank.Domain.Messaging;
using Sanlam.Chipo.Bank.Domain.Options;

namespace Sanlam.Chipo.Bank.Infrastructure.Sql.BankAccount;

/// <summary>
///   Implementation for IBankAccountRepository
/// </summary>
internal class BankAccountRepository(
    ILogger<BankAccountRepository> logger,
    IOptions<SqlConnectionsOptions> sqlOptions,
    ISqlConnectionFactory connectionFactory,
    IDistributedCacheService cacheService,
    IKafkaPublisherService kafkaPublisherService,
    IOptions<KafkaSettingOptions> kafkaOptions) : IBankAccountRepository
{
    /// <summary>Creates the withdraw outbox asynchronous.</summary>
    /// <param name="sessionKey">The session key.</param>
    /// <param name="accountNumber">The account number.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public async ValueTask<SystemActionResult<long>> CreateWithdrawOutboxAsync(
        string sessionKey,
        long accountNumber, 
        decimal amount,
        CancellationToken cancellationToken)
    {
        const string storedProcedure = "[BankAccount].[CreateWithdrawOutboxRequest]";

        var parameters = new Dictionary<string, object>
        {
            { "SessionKey", sessionKey },
            { "AccountNumber", accountNumber },
            { "Amount", amount },
            { "TimeUtc", DateTime.UtcNow },
            { "Status", ProcessStatus.New },
        };

        IDbConnection? connection = null;

        SystemActionError errorResult;

        try
        {
            connection = await connectionFactory.CreateConnectionAsync(sqlOptions.Value.BankConnection);

            var insertId = await connection.ExecuteScalarAsync<long>(
                new CommandDefinition(
                    commandText: storedProcedure,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken));

            return SystemActionResult.Success(insertId);
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, "Error getting balance: {AccountNumber}", accountNumber);

            await kafkaPublisherService.PublishMessage(
                kafkaOptions.Value.TopicWithdrawalEvents,
                new ModelSignificantEvent(
                    DateTime.UtcNow,
                    "Exception",
                    "CreateWithdrawOutbox",
                    ex.Message),
                cancellationToken);

            errorResult = new SystemActionError(
                500,
                BankAccountActionResult.DbError,
                $"A database exception occurred: {accountNumber}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting balance: {AccountNumber}", accountNumber);


            await kafkaPublisherService.PublishMessage(
                kafkaOptions.Value.TopicWithdrawalEvents,
                new ModelSignificantEvent(
                    DateTime.UtcNow,
                    "Exception",
                    "CreateWithdrawOutbox",
                    ex.Message),
                cancellationToken);


            errorResult = new SystemActionError(
                500,
                BankAccountActionResult.DbError,
                $"A database exception occurred: {accountNumber}");
        }
        finally
        {
            connection?.Dispose();
        }

        return SystemActionResult.Failure<long>(errorResult);
    }

    /// <summary>Gets the balance asynchronous.</summary>
    /// <param name="accountNumber">The account number.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public async ValueTask<SystemActionResult<ModelBankAccount>> GetBalanceAsync(
    long accountNumber,
    CancellationToken cancellationToken)
    {
        const string storedProcedure = "[BankAccount].[GetBalanceByAccountNumber]";

        var parameters = new Dictionary<string, object>
        {
            { "AccountNumber", accountNumber }
        };

        IDbConnection? connection = null;

        SystemActionError errorResult;

        try
        {
            connection = await connectionFactory.CreateConnectionAsync(sqlOptions.Value.BankConnection);

            var bankAccount = await connection.QueryFirstOrDefaultAsync<EntityBankAccount?>(
                new CommandDefinition(
                    commandText: storedProcedure,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken));

            if (bankAccount != null)
            {
                return SystemActionResult.Success(bankAccount.ToModel());
            }

            errorResult = new SystemActionError(
                400,
                BankAccountActionResult.NotFound,
                $"The account number specified was not found: {accountNumber}");
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, "Error getting balance: {AccountNumber}", accountNumber);

            await kafkaPublisherService.PublishMessage(
                kafkaOptions.Value.TopicWithdrawalEvents,
                new ModelSignificantEvent(
                    DateTime.UtcNow,
                    "Exception",
                    "GetBalance",
                    ex.Message),
                cancellationToken);


            errorResult = new SystemActionError(
                500,
                BankAccountActionResult.DbError,
                $"A database exception occurred: {accountNumber}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting balance: {AccountNumber}", accountNumber);

            await kafkaPublisherService.PublishMessage(
                kafkaOptions.Value.TopicWithdrawalEvents,
                new ModelSignificantEvent(
                    DateTime.UtcNow,
                    "Exception",
                    "GetBalance",
                    ex.Message),
                cancellationToken);

            errorResult = new SystemActionError(
                500,
                BankAccountActionResult.DbError,
                $"A database exception occurred: {accountNumber}");
        }
        finally
        {
            connection?.Dispose();
        }

        return SystemActionResult.Failure<ModelBankAccount>(errorResult);
    }


}