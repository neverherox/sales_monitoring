using SalesMonitoring.DML.Models;
using System.Data.Entity;

namespace SalesMonitoring.DAL.Context
{
    public class SalesContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        
    }
}
