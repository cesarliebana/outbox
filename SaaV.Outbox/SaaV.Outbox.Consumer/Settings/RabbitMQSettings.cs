using System.ComponentModel.DataAnnotations;

namespace SaaV.Outbox.Consumer.Settings
{
    public class RabbitMQSettings
    {
        [Required]
        public string? HostName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
