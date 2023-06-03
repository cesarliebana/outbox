using MediatR;
using SaaV.Outbox.Consumer.Persistence;

namespace SaaV.Outbox.Consumer.Domain
{
    public class CreateDummyHandler : IRequestHandler<CreateDummyRequest>
    {
        private readonly ConsumerDbContext _dbContext;

        public CreateDummyHandler(ConsumerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(CreateDummyRequest createDummyRequest, CancellationToken cancellationToken)
        {
            // Domain logic
            Dummy dummy = new(
                createDummyRequest.Id,
                createDummyRequest.Name,
                createDummyRequest.Number,
                createDummyRequest.CreatedDateTime);

            _dbContext.Dummies.Add(dummy);
            await _dbContext.SaveChangesAsync();
        }
    }
}
