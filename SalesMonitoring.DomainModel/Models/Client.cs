using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesMonitoring.DML.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required, MaxLength(30), MinLength(2)]
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public Client()
        {
            Orders = new List<Order>();
        }
    }
}
