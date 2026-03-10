using Sanlam.Chipo.Bank.Application.Models;

namespace Sanlam.Chipo.Bank.Infrastructure.Sql.BankAccount;

internal sealed record EntityBankAccount
{
    public long AccountNumber { get; set; }
    public DateTime OpenedTimeUct { get; set; }
    public decimal CurrentBalance { get; set; }

    public ModelBankAccount ToModel()
    {
        return new ModelBankAccount
        {
            AccountNumber = AccountNumber,
            OpenedTimeUct = OpenedTimeUct,
            CurrentBalance = CurrentBalance,
        };
    }
}