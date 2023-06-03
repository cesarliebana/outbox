namespace SaaV.Outbox.Producer.Domain
{
    public class Dummy
    {
        public int Id { get; set; }
        
        public string Name { get; private set; }

        public int Number { get; private set; }

        public DateTime CreatedDateTime { get; private set; }

        public Dummy()
        {
            Name = Guid.NewGuid().ToString();
            Number = new Random().Next(1, 100);
            CreatedDateTime = DateTime.UtcNow;
        }

    }
}
