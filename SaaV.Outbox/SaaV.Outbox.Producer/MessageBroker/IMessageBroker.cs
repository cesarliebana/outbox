namespace SaaV.Outbox.Producer.MessageBroker
{
    public interface IMessageBroker
    {
        void PublishMessage(string queueName, string message);
    }
}
