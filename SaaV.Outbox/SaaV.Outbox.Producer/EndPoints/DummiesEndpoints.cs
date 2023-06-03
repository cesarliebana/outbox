using MediatR;
using SaaV.Outbox.Producer.Domain;

namespace SaaV.Outbox.Producer.Endpoints
{
    internal static class DummiesEndpoints
    {
        public async static Task<IResult> CreateDummy(IMediator mediator)
        {
            CreateDummyRequest createDummyRequest = new();
            CreateDummyResponse createDummyResponse = await mediator.Send(createDummyRequest);
            
            return Results.Created($"/dummies/{createDummyResponse.Id}", createDummyResponse);
        }
    }
}
