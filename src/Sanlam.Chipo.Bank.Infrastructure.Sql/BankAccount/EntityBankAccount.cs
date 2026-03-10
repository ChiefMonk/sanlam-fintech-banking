using Sanlam.Chipo.Bank.Application.Models;

namespace Sanlam.Chipo.Bank.Infrastructure.Sql.BankAccount;

/// <summary>
///   Data Entity for BankAccount. Mirrors a SQL Table, specifically a  tuple
/// </summary>
internal sealed record EntityBankAccount
{
    public long AccountNumber { get; set; }
    public DateTime OpenedTimeUct { get; set; }
    public decimal CurrentBalance { get; set; }

    /// <summary>Converts to model.</summary>
    /// <returns>
    ///   <br />
    /// </returns>
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