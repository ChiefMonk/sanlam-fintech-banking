namespace Sanlam.Chipo.Bank.Domain.Enums;

/// <summary>
///   Enums for BankAccountActionResult
/// </summary>
public enum BankAccountActionResult
{
    UnknownError,
    NotFound,
    DbError,
    NotAvailable,
    InvalidAccount,
    InsufficientFunds,
    WithdrawalFailed,
    Successful
}