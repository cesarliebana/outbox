namespace SaaV.Outbox.Producer.Exceptions
{
    [Serializable]
    public class OutboxTransactionException : Exception
    {
        public OutboxTransactionException(Exception exception) : base($"Error in Transaction", exception)
        {
        }
    }
}
