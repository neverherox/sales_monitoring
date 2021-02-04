using System.Collections.Generic;

namespace SalesMonitoring.DML.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public Client()
        {
            Orders = new List<Order>();
        }
    }
}
