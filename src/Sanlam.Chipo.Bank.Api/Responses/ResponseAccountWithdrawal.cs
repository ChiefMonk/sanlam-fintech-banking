namespace Sanlam.Chipo.Bank.Api.Responses;

public sealed record ResponseAccountWithdrawal(
    long AccountNumber,
    DateTime TimeUtc,
    decimal Amount,
    decimal Balance);