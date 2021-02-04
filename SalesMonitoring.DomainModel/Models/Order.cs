using System;

namespace SalesMonitoring.DML.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual Client Client { get; set; }
        public virtual Product Product { get; set; }
    }
}
