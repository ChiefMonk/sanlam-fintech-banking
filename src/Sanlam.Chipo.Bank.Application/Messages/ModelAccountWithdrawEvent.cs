using Sanlam.Chipo.Bank.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sanlam.Chipo.Bank.Application.Messages;

public record ModelAccountWithdrawEvent(
    long AccountNumber,
    decimal Amount,
    DateTime TimeUtc,
    BankAccountActionResult Result,
    string Message);