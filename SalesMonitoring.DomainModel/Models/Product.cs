using System.Collections.Generic;

namespace SalesMonitoring.DML.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public Product()
        {
            Orders = new List<Order>();
        }
    }
}
