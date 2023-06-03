using Hangfire;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using SaaV.Outbox.Producer.Persistence;
using SaaV.Outbox.Producer.Shared;
using System.Text.Json;

namespace SaaV.Outbox.Producer.Domain
{
    public class CreateDummyHandler : IRequestHandler<CreateDummyRequest, CreateDummyResponse>
    {
        private readonly ProducerDbContext _dbContext;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public CreateDummyHandler(ProducerDbContext dbContext, IBackgroundJobClient backgroundJobClient)
        {
            _dbContext = dbContext;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<CreateDummyResponse> Handle(CreateDummyRequest createDummyRequest, CancellationToken cancellationToken)
        {
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            { 
                try
                {
                    // Domain logic
                    Dummy dummy = new();
                    _dbContext.Dummies.Add(dummy);
                    await _dbContext.SaveChangesAsync();

                    //Create response
                    CreateDummyResponse createDummyResponse = dummy.Adapt<CreateDummyResponse>();

                    // Create outbox message
                    string messagePayload = JsonSerializer.Serialize(createDummyResponse);
                    OutboxMessage outboxMessage = new(messagePayload);
                    _dbContext.OutboxMessages.Add(outboxMessage);
                    await _dbContext.SaveChangesAsync();

                    //Create background job
                    _backgroundJobClient.Enqueue<OutboxJob>(job => job.Execute(outboxMessage.Id));

                    // Commit transaction
                    await transaction.CommitAsync(cancellationToken);                    
                    return createDummyResponse;
                }
                catch (Exception)
                {
                    await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}
