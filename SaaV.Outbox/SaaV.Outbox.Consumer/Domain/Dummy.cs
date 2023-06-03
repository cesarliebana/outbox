namespace SaaV.Outbox.Consumer.Domain
{
    public class Dummy
    {
        public int Id { get; set; }
        
        public string Name { get; private set; }

        public int Number { get; private set; }

        public DateTime CreatedDateTime { get; private set; }

        public Dummy(int id, string name, int number, DateTime createdDateTime)
        {
            Id = id;
            Name = name;
            Number = number;
            CreatedDateTime = createdDateTime;
        }

    }
}
