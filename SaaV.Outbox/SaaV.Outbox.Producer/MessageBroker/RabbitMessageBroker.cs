using RabbitMQ.Client;
using System.Text;

namespace SaaV.Outbox.Producer.MessageBroker
{
    public class RabbitMQClient : IMessageBroker
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQClient(string hostName, string userName, string password)
        {
            var factory = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public RabbitMQClient(string hostName)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }


        public void PublishMessage(string queueName, string message)
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            byte[] body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}
