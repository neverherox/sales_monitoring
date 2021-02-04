using SalesMonitoring.DAL.Context.Configurations;
using SalesMonitoring.DML.Models;
using System.Data.Entity;

namespace SalesMonitoring.DAL.Context
{
    public class SalesContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public SalesContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SalesContext, Migrations.Configuration>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClientConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
        }
    }
}
