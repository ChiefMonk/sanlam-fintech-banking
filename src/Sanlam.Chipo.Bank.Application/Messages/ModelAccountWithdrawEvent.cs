using Sanlam.Chipo.Bank.Domain.Enums;

namespace Sanlam.Chipo.Bank.Application.Messages;

/// <summary>
///  Model for AccountWithdrawEvent
/// </summary>
public record ModelAccountWithdrawEvent(
    long AccountNumber,
    decimal Amount,
    DateTime TimeUtc,
    BankAccountActionResult Result,
    string Message);