using Hangfire;
using Microsoft.EntityFrameworkCore;
using SaaV.Outbox.Producer.MessageBroker;
using SaaV.Outbox.Producer.Persistence;

namespace SaaV.Outbox.Producer.Shared
{
    public class OutboxJob
    {
        private readonly ILogger<OutboxJob> _logger;
        private readonly ProducerDbContext _dbContext;
        private readonly IMessageBroker _messageBroker;

        public OutboxJob(ILogger<OutboxJob> logger, ProducerDbContext dbContext, IMessageBroker messageBroker)
        {
            _logger = logger;
            _dbContext = dbContext;
            _messageBroker = messageBroker;
    }

        [Queue("outbox-queue")]
        public async Task Execute(Guid id)
        {
            _logger.LogInformation($"Init outbox job process! [Guid: {id}]");

            OutboxMessage? outboxMessage = await _dbContext.OutboxMessages.FirstOrDefaultAsync(message => message.Id == id);
            if (outboxMessage != null)
            {
                _logger.LogInformation($"Publishing message to queue: {outboxMessage.Payload}");
                _messageBroker.PublishMessage("dummy-queue", outboxMessage.Payload);

                outboxMessage.MarkAsProcessed();
                await _dbContext.SaveChangesAsync();
            }

            _logger.LogInformation($"Finish outbox job process! [Guid: {id}]");
        }
    }
}
