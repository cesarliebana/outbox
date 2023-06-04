using MediatR;
using SaaV.Outbox.Consumer.Domain;
using SaaV.Outbox.Consumer.MessageBroker;
using SaaV.Outbox.Consumer.Persistence;
using System.Text.Json;

namespace SaaV.Outbox.Consumer.Events
{
    public class CreateDummyEventHandler: BackgroundService
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IMediator _mediator;
        private readonly IServiceProvider _services;

        public CreateDummyEventHandler(IServiceProvider services, IMessageBroker messageBroker, IMediator mediator)
        {
            _messageBroker = messageBroker;
            _mediator = mediator;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _services.CreateScope())
            {
                IRequestHandler<CreateDummyRequest> createDumyHandler = scope.ServiceProvider.GetRequiredService<IRequestHandler<CreateDummyRequest>>();
                _messageBroker.ConsumeMessage("dummy-queue", async message =>
                {
                    Console.WriteLine($"Received message: {message}");
                    CreateDummyRequest createDummyRequest = JsonSerializer.Deserialize<CreateDummyRequest>(message);

                    await createDumyHandler.Handle(createDummyRequest, stoppingToken);
                });
            }
        }
    }
}
