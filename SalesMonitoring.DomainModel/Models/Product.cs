using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesMonitoring.DML.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(30), MinLength(2)]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public Product()
        {
            Orders = new List<Order>();
        }
    }
}
