namespace Sanlam.Chipo.Bank.Domain.Messaging;

public interface IRabbitMqPublisherService
{
    Task PublishMessage(
        string exchangeName,
        string routingKey,
        object message,
        CancellationToken cancellationToken);
}