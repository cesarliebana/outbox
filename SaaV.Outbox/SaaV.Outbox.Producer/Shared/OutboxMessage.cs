namespace SaaV.Outbox.Producer.Shared
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public bool IsProcessed { get; set; }

        public OutboxMessage(string payload)
        {
            Id = Guid.NewGuid();
            Payload = payload;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsProcessed()
        {
            ProcessedAt = DateTime.UtcNow;
            IsProcessed = true;
        }
    }
}
