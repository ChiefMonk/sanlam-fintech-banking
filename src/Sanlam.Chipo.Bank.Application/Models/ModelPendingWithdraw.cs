using Sanlam.Chipo.Bank.Domain.Enums;

namespace Sanlam.Chipo.Bank.Application.Models;

public record ModelPendingWithdraw
{
    public Guid ReferenceNumber { get; set; }
    public long AccountNumber { get; set; }
    public DateTime WithdrawTimeUct { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }

    public ProcessStatus Status { get; set; }
}