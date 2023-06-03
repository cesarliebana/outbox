using MediatR;

namespace SaaV.Outbox.Producer.Domain
{
    public record struct CreateDummyRequest() : IRequest<CreateDummyResponse>;
}
