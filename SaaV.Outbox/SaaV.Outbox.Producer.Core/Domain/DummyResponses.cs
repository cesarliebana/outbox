namespace SaaV.Outbox.Producer.Core.Domain
{
    public record struct CreateDummyResponse(int Id, string Name, int Number, DateTime CreatedDateTime);
}
