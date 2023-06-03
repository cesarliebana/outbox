using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SaaV.Outbox.Producer.Domain;
using SaaV.Outbox.Producer.Shared;

namespace SaaV.Outbox.Producer.Persistence
{
    public class ProducerDbContext : DbContext
    {
        public ProducerDbContext(DbContextOptions<ProducerDbContext> options) : base(options)
        {
        }

        #region DbSets
        public virtual DbSet<Dummy> Dummies { get; set; }
        public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Global Conventios
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());
               
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    if (property.ClrType.Equals(typeof(DateTime))) property.SetColumnType("datetime");
                    else if (property.ClrType.Equals(typeof(decimal))) property.SetColumnType("decimal(10,2)");
                    else if (property.ClrType.Equals(typeof(string)))
                    {
                        property.IsNullable = false;
                        property.SetMaxLength(255);
                        property.SetIsUnicode(true);
                    }
                }
            }
            #endregion

            #region Entities
            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.Property(e => e.Payload).HasMaxLength(2048).IsRequired();
            });
            #endregion
        }
    }
}
