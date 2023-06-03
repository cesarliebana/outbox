using MediatR;

namespace SaaV.Outbox.Producer.Core.Domain
{
    public record struct CreateDummyRequest() : IRequest<GetDummyResponse>;
}
