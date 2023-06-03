using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SaaV.Outbox.Producer.Core.Persistence
{
    public class ProducerDbContextFactory : IDesignTimeDbContextFactory<ProducerDbContext>
    {
        public ProducerDbContext CreateDbContext(string[]? args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<ProducerDbContextFactory>()
            .Build();

            DbContextOptionsBuilder<ProducerDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer(config.GetConnectionString("ProducerConnectionString"));

            return new ProducerDbContext(optionsBuilder.Options);
        }
    }
}
