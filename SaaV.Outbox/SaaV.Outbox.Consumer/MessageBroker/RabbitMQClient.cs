﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace SaaV.Outbox.Consumer.MessageBroker
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

        public void ConsumeMessage(string queueName, Action<string> handleMessage)
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            EventingBasicConsumer consumer = new(_channel);
            consumer.Received += (model, eventArguments) =>
            {
                ReadOnlyMemory<byte> body = eventArguments.Body;
                string message = Encoding.UTF8.GetString(body.ToArray());
                handleMessage(message);
            };
            
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
