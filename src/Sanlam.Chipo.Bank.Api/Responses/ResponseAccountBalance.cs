namespace Sanlam.Chipo.Bank.Api.Responses;

/// <summary>
///    Response for AccountBalance
/// </summary>
public sealed record ResponseAccountBalance(
    long AccountNumber,
    DateTime TimeUtc,
    decimal Balance);