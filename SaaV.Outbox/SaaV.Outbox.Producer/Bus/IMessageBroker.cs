namespace SaaV.Outbox.Producer.Bus
{
    public interface IMessageBroker
    {
        void PublishMessage(string queueName, string message);
    }
}
