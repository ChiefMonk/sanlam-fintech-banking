namespace Sanlam.Chipo.Bank.Domain.Messaging;

public interface IKafkaPublisherService
{
    Task PublishMessage(
        string topicName,
        object message,
        CancellationToken cancellationToken);
}