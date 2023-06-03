namespace SaaV.Outbox.Consumer.MessageBroker
{
    public interface IMessageBroker
    {
        void PublishMessage(string queueName, string message);

        void ConsumeMessage(string queueName, Action<string> handleMessage);
    }
}
