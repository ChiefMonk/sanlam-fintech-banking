namespace Sanlam.Chipo.Bank.Application.Models;

public record ModelBankAccount
{
    public long AccountNumber { get; set; }

    public DateTime OpenedTimeUct { get; set; }

    public decimal CurrentBalance { get; set; }
}