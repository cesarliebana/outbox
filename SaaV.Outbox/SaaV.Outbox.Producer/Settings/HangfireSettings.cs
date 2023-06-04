using System.ComponentModel.DataAnnotations;

namespace SaaV.Outbox.Producer.Settings
{
    public class HangfireSettings
    {
        [Required]
        public int MaxWorkers { get; set; }
    }
}
