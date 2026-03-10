namespace Sanlam.Chipo.Bank.Domain.Messaging;

public interface IRabbitMqPublisherService
{
    /// <summary>Publishes the message.</summary>
    /// <param name="exchangeName">Name of the exchange.</param>
    /// <param name="routingKey">The routing key.</param>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    Task PublishMessage(
        string exchangeName,
        string routingKey,
        object message,
        CancellationToken cancellationToken);
}