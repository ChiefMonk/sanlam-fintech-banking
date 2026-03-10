namespace Sanlam.Chipo.Bank.Application.Models;

/// <summary>
///   Model for AccountBalance
/// </summary>
public record ModelAccountBalance
{
    /// <summary>Gets or sets the account number.</summary>
    /// <value>The account number.</value>
    public long AccountNumber { get; set; }

    /// <summary>Gets or sets the time uct.</summary>
    /// <value>The time uct.</value>
    public DateTime TimeUct { get; set; }

    /// <summary>Gets or sets the balance amount.</summary>
    /// <value>The balance amount.</value>
    public decimal BalanceAmount { get; set; }
}