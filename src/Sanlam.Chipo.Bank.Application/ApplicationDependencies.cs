using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sanlam.Chipo.Bank.Application.Services.BankAccount;

namespace Sanlam.Chipo.Bank.Application;

public static class ApplicationDependencies
{
    public static IServiceCollection RegisterApplicationDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddScoped<IBankingAccountService, BankingAccountService>();
        return services;
    }
}