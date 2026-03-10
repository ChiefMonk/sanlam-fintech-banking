using System.Data;

namespace Sanlam.Chipo.Bank.Infrastructure.Sql.Connection;

internal interface ISqlConnectionFactory
{
    /// <summary>Creates the connection asynchronous.</summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    Task<IDbConnection> CreateConnectionAsync(string connectionString);
}