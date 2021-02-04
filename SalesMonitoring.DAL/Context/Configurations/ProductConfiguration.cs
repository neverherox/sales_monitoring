using SalesMonitoring.DML.Models;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesMonitoring.DAL.Context.Configurations
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            this.ToTable("Products")
                .HasKey(x => x.Id)
                .HasMany(x => x.Orders)
                .WithRequired(y => y.Product);

            this.Property(x => x.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(x => x.Name)
                .HasMaxLength(30)
                .IsRequired();

            this.Property(x => x.Price)
                .IsRequired();
        }
    }
}
