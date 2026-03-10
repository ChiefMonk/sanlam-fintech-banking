namespace Sanlam.Chipo.Bank.Application.Models;

public record ModelAccountWithdraw
{
    public long AccountNumber { get; set; }

    public DateTime TimeUct { get; set; }

    public decimal WithdrawAmount { get; set; }

    public decimal BalanceAmount { get; set; }
}