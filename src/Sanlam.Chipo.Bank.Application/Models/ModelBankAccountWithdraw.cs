namespace Sanlam.Chipo.Bank.Application.Models;

public record ModelBankAccountWithdraw
{
    public Guid ReferenceNumber { get; set; }
    public long AccountNumber { get; set; }

    public DateTime DateTimeWithdrawUct { get; set; }
    public decimal AmountWithdrawn { get; set; }

    public decimal BalanceAfterDraw { get; set; }
}