namespace Sanlam.Chipo.Bank.Application.Models;

/// <summary>
///   Model for BankAccount
/// </summary>
public record ModelBankAccount
{
    /// <summary>Gets or sets the account number.</summary>
    /// <value>The account number.</value>
    public long AccountNumber { get; set; }

    /// <summary>Gets or sets the opened time uct.</summary>
    /// <value>The opened time uct.</value>
    public DateTime OpenedTimeUct { get; set; }

    /// <summary>Gets or sets the current balance.</summary>
    /// <value>The current balance.</value>
    public decimal CurrentBalance { get; set; }
}