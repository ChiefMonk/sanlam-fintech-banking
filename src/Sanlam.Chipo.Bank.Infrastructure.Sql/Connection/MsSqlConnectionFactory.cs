using Microsoft.Data.SqlClient;
using System.Data;

namespace Sanlam.Chipo.Bank.Infrastructure.Sql.Connection;

internal class MsSqlConnectionFactory: ISqlConnectionFactory
{
    /// <summary>Creates the connection asynchronous.</summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public async Task<IDbConnection> CreateConnectionAsync(string connectionString)
    {
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(10));
        var cancellationToken = tokenSource.Token;

        var  connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}