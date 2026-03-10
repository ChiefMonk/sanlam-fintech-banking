namespace Sanlam.Chipo.Bank.Api.Responses;

/// <summary>
///   Response for AccountWithdrawal
/// </summary>
public sealed record ResponseAccountWithdrawal(
    long AccountNumber,
    DateTime TimeUtc,
    decimal Amount,
    decimal Balance);