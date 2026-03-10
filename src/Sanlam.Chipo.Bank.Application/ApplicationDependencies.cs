using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sanlam.Chipo.Bank.Application.Services.BankAccount;

namespace Sanlam.Chipo.Bank.Application;

public static class ApplicationDependencies
{
    /// <summary>Registers the application dependencies.</summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static IServiceCollection RegisterApplicationDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddScoped<IBankingAccountService, BankingAccountService>();
        return services;
    }
}