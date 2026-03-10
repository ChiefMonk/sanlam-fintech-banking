using Microsoft.Extensions.Logging;
using Sanlam.Chipo.Bank.Domain.Messaging;

namespace Sanlam.Chipo.Bank.Infrastructure.Messaging;

/// <summary>
///  Implementation for IKafkaPublisherService
/// </summary>
internal class KafkaPublisherService(
    ILogger<KafkaPublisherService> logger) : IKafkaPublisherService
{
    /// <summary>Publishes the message.</summary>
    /// <param name="topicName">Name of the topic.</param>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public Task PublishMessage(
        string topicName, 
        object message, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Publish Message to Kafka: {TopicName}", topicName);

        return Task.FromResult(true);
    }
}