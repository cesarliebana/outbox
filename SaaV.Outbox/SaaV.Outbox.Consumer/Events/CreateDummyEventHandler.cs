using Azure.Core.Serialization;
using MediatR;
using Microsoft.Extensions.Hosting;
using SaaV.Outbox.Consumer.Domain;
using SaaV.Outbox.Consumer.MessageBroker;
using System.Text.Json;

namespace SaaV.Outbox.Consumer.Events
{
    public class CreateDummyEventHandler: BackgroundService
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IMediator _mediator;

        public CreateDummyEventHandler(IMessageBroker messageBroker, IMediator mediator)
        {
            _messageBroker = messageBroker;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messageBroker.ConsumeMessage("dummy-queue", async message => 
            {
                Console.WriteLine($"Received message: {message}");
                CreateDummyRequest createDummyRequest = JsonSerializer.Deserialize<CreateDummyRequest>(message);
                await _mediator.Send(createDummyRequest);
            });
        }
    }
}
