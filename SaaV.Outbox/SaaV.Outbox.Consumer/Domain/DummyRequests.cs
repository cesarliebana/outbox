using MediatR;

namespace SaaV.Outbox.Consumer.Domain
{
    public record struct CreateDummyRequest(int Id, string Name, int Number, DateTime CreatedDateTime): IRequest;
}
