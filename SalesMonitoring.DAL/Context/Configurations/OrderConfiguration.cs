using SalesMonitoring.DML.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalesMonitoring.DAL.Context.Configurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            this.ToTable("Orders")
                .HasKey(x => x.Id);       

            this.Property(x => x.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(x => x.Date)
                .IsRequired();
        }
    }
}
