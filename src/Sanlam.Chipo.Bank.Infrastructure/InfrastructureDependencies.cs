using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sanlam.Chipo.Bank.Domain.Caching;
using Sanlam.Chipo.Bank.Domain.Messaging;
using Sanlam.Chipo.Bank.Domain.Options;
using Sanlam.Chipo.Bank.Infrastructure.Caching;
using Sanlam.Chipo.Bank.Infrastructure.Messaging;
using StackExchange.Redis;

namespace Sanlam.Chipo.Bank.Infrastructure;

public static class InfrastructureDependencies
{
    /// <summary>Registers the infrastructure dependencies.</summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static IServiceCollection RegisterInfrastructureDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IDistributedCacheService>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<RedisDistributedCacheService>>();
            var redisOptions = provider.GetRequiredService<IOptions<RedisSettingOptions>>();

            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer
                .Connect(redisOptions.Value.ConnectionString);
            var genericCacheOptions = new RedisCacheOptions
            {
                ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer),
                InstanceName = "redis"
            };

            return new RedisDistributedCacheService(
                logger,
                redisOptions,
                new RedisCache(genericCacheOptions),
                connectionMultiplexer);
        });

        services.AddTransient<IDistributedLockService, RedisDistributedLockService>();

        services.AddScoped<IKafkaPublisherService, KafkaPublisherService>();
        services.AddScoped<IRabbitMqPublisherService, RabbitMqPublisherService>();

        return services;
    }
}