using Microsoft.Extensions.Logging;
using Sanlam.Chipo.Bank.Domain.Messaging;

namespace Sanlam.Chipo.Bank.Infrastructure.Messaging;

internal class RabbitMqPublisherService(
    ILogger<RabbitMqPublisherService> logger) : IRabbitMqPublisherService
{
    public Task PublishMessage(
        string topicName, 
        object message, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Publish Message to Kafka: {TopicName}", topicName);

        return Task.FromResult(true);
    }

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