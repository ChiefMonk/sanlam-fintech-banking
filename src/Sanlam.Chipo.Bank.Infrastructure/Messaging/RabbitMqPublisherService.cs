using Microsoft.Extensions.Logging;
using Sanlam.Chipo.Bank.Domain.Messaging;

namespace Sanlam.Chipo.Bank.Infrastructure.Messaging;

/// <summary>
///    Implementation for IKafkaPublisherService
/// </summary>
internal class RabbitMqPublisherService(
    ILogger<RabbitMqPublisherService> logger) : IRabbitMqPublisherService
{
    /// <summary>Publishes the message.</summary>
    /// <param name="exchangeName">Name of the exchange.</param>
    /// <param name="routingKey">The routing key.</param>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public Task PublishMessage(
        string exchangeName, 
        string routingKey, 
        object message, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Publish Message to RabbitMQ: {ExchangeName}:{RoutingKey}", exchangeName, routingKey);

        return Task.FromResult(true);
    }
}