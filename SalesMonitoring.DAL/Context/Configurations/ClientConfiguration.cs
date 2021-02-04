using SalesMonitoring.DML.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalesMonitoring.DAL.Context.Configurations
{
    public class ClientConfiguration : EntityTypeConfiguration<Client>
    {
        public ClientConfiguration()
        {
            this.ToTable("Clients")
                .HasKey(x => x.Id)
                .HasMany(x => x.Orders)
                .WithRequired(y => y.Client);

            this.Property(x => x.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(x => x.Name)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
