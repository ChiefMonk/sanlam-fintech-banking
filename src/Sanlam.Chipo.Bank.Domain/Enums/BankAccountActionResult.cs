namespace Sanlam.Chipo.Bank.Domain.Enums;

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