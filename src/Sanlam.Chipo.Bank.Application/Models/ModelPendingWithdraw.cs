using Sanlam.Chipo.Bank.Domain.Enums;

namespace Sanlam.Chipo.Bank.Application.Models;

public record ModelPendingWithdraw
{
    /// <summary>Gets or sets the reference number.</summary>
    /// <value>The reference number.</value>
    public Guid ReferenceNumber { get; set; }
    /// <summary>Gets or sets the account number.</summary>
    /// <value>The account number.</value>
    public long AccountNumber { get; set; }
    /// <summary>Gets or sets the withdraw time uct.</summary>
    /// <value>The withdraw time uct.</value>
    public DateTime WithdrawTimeUct { get; set; }
    /// <summary>Gets or sets the amount.</summary>
    /// <value>The amount.</value>
    public decimal Amount { get; set; }
    /// <summary>Gets or sets the balance after.</summary>
    /// <value>The balance after.</value>
    public decimal BalanceAfter { get; set; }

    /// <summary>Gets or sets the status.</summary>
    /// <value>The status.</value>
    public ProcessStatus Status { get; set; }
}