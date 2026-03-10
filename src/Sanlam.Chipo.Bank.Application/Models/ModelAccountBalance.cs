namespace Sanlam.Chipo.Bank.Application.Models;

public record ModelAccountBalance
{
    public long AccountNumber { get; set; }

    public DateTime TimeUct { get; set; }

    public decimal BalanceAmount { get; set; }
}