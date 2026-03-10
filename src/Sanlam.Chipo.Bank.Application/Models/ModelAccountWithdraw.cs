namespace Sanlam.Chipo.Bank.Application.Models;

/// <summary>
///   Model for AccountWithdraw
/// </summary>
public record ModelAccountWithdraw
{
    /// <summary>Gets or sets the account number.</summary>
    /// <value>The account number.</value>
    public long AccountNumber { get; set; }

    /// <summary>Gets or sets the time uct.</summary>
    /// <value>The time uct.</value>
    public DateTime TimeUct { get; set; }

    /// <summary>Gets or sets the withdraw amount.</summary>
    /// <value>The withdraw amount.</value>
    public decimal WithdrawAmount { get; set; }

    /// <summary>Gets or sets the balance amount.</summary>
    /// <value>The balance amount.</value>
    public decimal BalanceAmount { get; set; }
}