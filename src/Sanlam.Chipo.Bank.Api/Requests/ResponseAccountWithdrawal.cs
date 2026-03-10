namespace Sanlam.Chipo.Bank.Api.Requests;

/// <summary>
///   Request for AccountWithdrawal
/// </summary>
public sealed record RequestAccountWithdrawal(
    long AccountNumber,
    decimal Amount);