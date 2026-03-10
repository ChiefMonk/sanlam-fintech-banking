namespace Sanlam.Chipo.Bank.Application.Models;

/// <summary>
///   Model for BankAccountWithdraw
/// </summary>
public record ModelBankAccountWithdraw
{
    /// <summary>Gets or sets the reference number.</summary>
    /// <value>The reference number.</value>
    public Guid ReferenceNumber { get; set; }
    /// <summary>Gets or sets the account number.</summary>
    /// <value>The account number.</value>
    public long AccountNumber { get; set; }

    public DateTime DateTimeWithdrawUct { get; set; }
    /// <summary>Gets or sets the amount withdrawn.</summary>
    /// <value>The amount withdrawn.</value>
    public decimal AmountWithdrawn { get; set; }

    /// <summary>Gets or sets the balance after draw.</summary>
    /// <value>The balance after draw.</value>
    public decimal BalanceAfterDraw { get; set; }
}