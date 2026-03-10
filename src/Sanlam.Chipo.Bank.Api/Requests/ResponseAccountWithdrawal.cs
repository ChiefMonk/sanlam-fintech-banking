namespace Sanlam.Chipo.Bank.Api.Requests;

public sealed record RequestAccountWithdrawal(
    long AccountNumber,
    decimal Amount);