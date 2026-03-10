using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sanlam.Chipo.Bank.Domain.Options;
using Sanlam.Chipo.Bank.Domain.Session;

namespace Sanlam.Chipo.Bank.Domain;

public static class DomainDependencies
{
    public static IServiceCollection RegisterDomainDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddConfigOptions(configuration);

        services.AddScoped<IUserSessionContext, UserSessionContext>();

        return services;
    }

    private static void AddConfigOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // OpenTelemetry
        services
            .AddOptions<OpenTelemetryOptions>()
            .Bind(configuration.GetSection(OpenTelemetryOptions.ConfigName))
            .ValidateDataAnnotations()
            .Validate(o => o.IsValid(),
                "OpenTelemetry configurations are missing or invalid.")
            .ValidateOnStart();

        // Redis
        services
            .AddOptions<RedisSettingOptions>()
            .Bind(configuration.GetSection(RedisSettingOptions.ConfigName))
            .ValidateDataAnnotations()
            .Validate(o => o.IsValid(),
                "Redis configurations are missing or invalid.")
            .ValidateOnStart();

        // SqlConnections
        services
            .AddOptions<SqlConnectionsOptions>()
            .Bind(configuration.GetSection(SqlConnectionsOptions.ConfigName))
            .ValidateDataAnnotations()
            .Validate(o => o.IsValid(),
                "SqlConnections configurations are missing or invalid.")
            .ValidateOnStart();

        // NoSqlConnections
        services
            .AddOptions<NoSqlConnectionsOptions>()
            .Bind(configuration.GetSection(NoSqlConnectionsOptions.ConfigName))
            .ValidateDataAnnotations()
            .Validate(o => o.IsValid(),
                "NoSqlConnections configurations are missing or invalid.")
            .ValidateOnStart();

        // NoSqlConnections
        services
            .AddOptions<NoSqlConnectionsOptions>()
            .Bind(configuration.GetSection(NoSqlConnectionsOptions.ConfigName))
            .ValidateDataAnnotations()
            .Validate(o => o.IsValid(),
                "NoSqlConnections configurations are missing or invalid.")
            .ValidateOnStart();

        // Redis
        services
            .AddOptions<RedisSettingOptions>()
            .Bind(configuration.GetSection(RedisSettingOptions.ConfigName))
            .ValidateDataAnnotations()
            .Validate(o => o.IsValid(),
                "Redis configurations are missing or invalid.")
            .ValidateOnStart();

        // Kafka
        services
            .AddOptions<KafkaSettingOptions>()
            .Bind(configuration.GetSection(KafkaSettingOptions.ConfigName))
            .ValidateDataAnnotations()
            .Validate(o => o.IsValid(),
                "Kafka configurations are missing or invalid.")
            .ValidateOnStart();
    }
}