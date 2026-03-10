using Microsoft.Extensions.Logging;
using Sanlam.Chipo.Bank.Domain.Messaging;

namespace Sanlam.Chipo.Bank.Infrastructure.Messaging;

internal class KafkaPublisherService(
    ILogger<KafkaPublisherService> logger) : IKafkaPublisherService
{
    public Task PublishMessage(
        string topicName, 
        object message, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Publish Message to Kafka: {TopicName}", topicName);

        return Task.FromResult(true);
    }
}