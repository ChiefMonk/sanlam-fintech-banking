namespace Sanlam.Chipo.Bank.Api.Responses;

public sealed record ResponseAccountBalance(
    long AccountNumber,
    DateTime TimeUtc,
    decimal Balance);