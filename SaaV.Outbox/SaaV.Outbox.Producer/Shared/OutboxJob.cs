using Hangfire;
using Microsoft.EntityFrameworkCore;
using SaaV.Outbox.Producer.Persistence;

namespace SaaV.Outbox.Producer.Shared
{
    public class OutboxJob
    {
        private readonly ILogger<OutboxJob> _logger;
        private readonly ProducerDbContext _dbContext;

        public OutboxJob(ILogger<OutboxJob> logger, ProducerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
    }

        [Queue("outbox-queue")]
        public async Task Execute(Guid id)
        {
            _logger.LogInformation($"Init outbox job process! [Guid: {id}]");
            
            OutboxMessage? outboxMessage = await _dbContext.OutboxMessages.FirstOrDefaultAsync(message => message.Id == id);
            if (outboxMessage != null)
            {
                outboxMessage.MarkAsProcessed();
                await _dbContext.SaveChangesAsync();
            }

            _logger.LogInformation($"Finish outbox job process! [Guid: {id}]");
        }
    }
}
