using System.Data;

namespace Sanlam.Chipo.Bank.Infrastructure.Sql.Connection;

internal interface ISqlConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(string connectionString);
}