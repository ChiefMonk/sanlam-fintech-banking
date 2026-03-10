using Microsoft.AspNetCore.Mvc;
using Sanlam.Chipo.Bank.Api.Requests;
using Sanlam.Chipo.Bank.Api.Responses;
using Sanlam.Chipo.Bank.Application.Services.BankAccount;
using System.Net.Mime;

namespace Sanlam.Chipo.Bank.Api.Controllers;

[ApiController]
[Route("bank")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class BankingController(
    ILogger<BankingController> logger,
        IBankingAccountService bankAccountService) : ControllerBase
{
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

    private CancellationToken CreateCancellationToken(int timeoutSecs = 10)
    {
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(timeoutSecs));
        return tokenSource.Token;
    }
}