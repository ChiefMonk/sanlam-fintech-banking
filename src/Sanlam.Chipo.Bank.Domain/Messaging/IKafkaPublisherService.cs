namespace Sanlam.Chipo.Bank.Domain.Messaging;

public interface IKafkaPublisherService
{
    /// <summary>Publishes the message.</summary>
    /// <param name="topicName">Name of the topic.</param>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    Task PublishMessage(
        string topicName,
        object message,
        CancellationToken cancellationToken);
}