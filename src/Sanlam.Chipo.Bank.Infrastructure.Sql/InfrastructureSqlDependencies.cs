using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sanlam.Chipo.Bank.Application.Repositories;
using Sanlam.Chipo.Bank.Infrastructure.Sql.BankAccount;
using Sanlam.Chipo.Bank.Infrastructure.Sql.Connection;

namespace Sanlam.Chipo.Bank.Infrastructure.Sql;

public static class InfrastructureSqlDependencies
{
    /// <summary>Registers the infrastructure SQL dependencies.</summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static IServiceCollection RegisterInfrastructureSqlDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
     
        services.AddSingleton<ISqlConnectionFactory>(new MsSqlConnectionFactory());
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        return services;
    }
}