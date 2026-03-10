using Microsoft.AspNetCore.Mvc;
using Sanlam.Chipo.Bank.Api.Requests;
using Sanlam.Chipo.Bank.Api.Responses;
using Sanlam.Chipo.Bank.Application.Services.BankAccount;
using System.Net.Mime;

namespace Sanlam.Chipo.Bank.Api.Controllers;

/// <summary>
///   BankingController: A WebAPi for the bank service
/// </summary>
[ApiController]
[Route("bank")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class BankingController(
    ILogger<BankingController> logger,
        IBankingAccountService bankAccountService) : ControllerBase
{
    /// <summary>Accounts the withdraw.</summary>
    /// <param name="withdrawalRequest">The withdrawal request.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    [HttpPut("withdraw", Name = "account-withdraw")]
    public async Task<ActionResult<ResponseAccountBalance>> AccountWithdraw(
        [FromBody] RequestAccountWithdrawal withdrawalRequest)
    {
        var cancellationToken = CreateCancellationToken();

        logger.LogInformation("Request for Account-Withdraw: {AccountNumber}:{Amount}", 
            withdrawalRequest.AccountNumber,
            withdrawalRequest.Amount);

        var withdrawResult = await bankAccountService
            .WithdrawAsync(
                accountNumber: withdrawalRequest.AccountNumber,
                amount: withdrawalRequest.Amount,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (withdrawResult.IsSuccess)
        {
            return Ok(new ResponseAccountWithdrawal(
                withdrawResult.Value.AccountNumber,
                withdrawResult.Value.TimeUct,
                withdrawResult.Value.WithdrawAmount,
                withdrawResult.Value.BalanceAmount));
        }

        logger.LogWarning("Balance check failed for: {AccountNumber}:{Amount}: {Error}",
            withdrawalRequest.AccountNumber,
            withdrawalRequest.Amount,
            withdrawResult.Error);

        return BadRequest(withdrawResult.Error);
    }


    /// <summary>Accounts the balance check.</summary>
    /// <param name="accountNumber">The account number.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    [HttpGet("balance/{accountNumber:long}", Name = "account-balance-balance")]
    public async Task<ActionResult<ResponseAccountBalance>> AccountBalanceCheck(
        [FromQuery] long accountNumber)
    {
        var cancellationToken = CreateCancellationToken();

        logger.LogInformation("Request for AccountC-Balance-Check: {AccountNumber}", accountNumber);

        var balanceResult = await bankAccountService
            .GetBalanceAsync(
                accountNumber: accountNumber,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (balanceResult.IsSuccess)
        {
            return Ok(new ResponseAccountBalance(
                balanceResult.Value.AccountNumber,
                balanceResult.Value.TimeUct,
                balanceResult.Value.BalanceAmount));
        }

        logger.LogWarning("Balance check failed for {AccountNumber}: {Error}",
            accountNumber, balanceResult.Error);

        return BadRequest(balanceResult.Error);
    }

    /// <summary>Creates the cancellation token.</summary>
    /// <param name="timeoutSecs">The timeout secs.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    private CancellationToken CreateCancellationToken(int timeoutSecs = 10)
    {
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(timeoutSecs));
        return tokenSource.Token;
    }
}