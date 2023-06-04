using MediatR;
using SaaV.Outbox.Consumer.Domain;
using SaaV.Outbox.Consumer.MessageBroker;
using SaaV.Outbox.Consumer.Persistence;
using System.Text.Json;

namespace SaaV.Outbox.Consumer.Events
{
    public class EventsHandlerService: BackgroundService
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IServiceProvider _services;

        public EventsHandlerService(IServiceProvider services, IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
            _services = services;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messageBroker.ConsumeMessage("dummy-queue", async message =>
            {
                using (IServiceScope scope = _services.CreateScope())
                {
                    Console.WriteLine($"Received message: {message}");
                    CreateDummyRequest createDummyRequest = JsonSerializer.Deserialize<CreateDummyRequest>(message);
                    IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(createDummyRequest, stoppingToken);
                }
            });

            return Task.CompletedTask;
        }
    }
}
