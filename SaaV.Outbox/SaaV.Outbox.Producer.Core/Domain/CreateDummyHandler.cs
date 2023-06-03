using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaV.Outbox.Producer.Core.Persistence;
using SaaV.Outbox.Producer.Core.Shared;
using System.Transactions;

namespace SaaV.Outbox.Producer.Core.Domain
{
    public class CreateDummyHandler : IRequestHandler<CreateDummyRequest, CreateDummyResponse>
    {
        private readonly ProducerDbContext _dbContext;

        public CreateDummyHandler(ProducerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateDummyResponse> Handle(CreateDummyRequest createDummyRequest, CancellationToken cancellationToken)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Aquí realizamos nuestras operaciones normales de base de datos...

            var outboxMessage = new OutboxMessage
            {
                Id = messageId,
                Payload = messagePayload,
            };

            _context.OutboxMessages.Add(outboxMessage);
            await _context.SaveChangesAsync();

            scope.Complete();

            Dummy dummy = new();
            _dbContext.Dummies.Add(dummy);
            
            return dummy.Adapt<CreateDummyResponse>();
        }
    }
}
