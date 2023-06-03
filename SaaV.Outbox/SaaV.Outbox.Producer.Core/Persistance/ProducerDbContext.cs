using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SaaV.Outbox.Producer.Core.Domain;

namespace SaaV.Outbox.Producer.Core.Persistence
{
    public class ProducerDbContext : DbContext
    {
        public ProducerDbContext(DbContextOptions<ProducerDbContext> options) : base(options)
        {
        }

        #region DbSets
        public virtual DbSet<Dummy> Dummies { get; set; }
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
        }
    }
}
