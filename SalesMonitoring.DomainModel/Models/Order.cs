using System;
using System.ComponentModel.DataAnnotations;

namespace SalesMonitoring.DML.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public virtual Client Client { get; set; }
        [Required]
        public virtual Product Product { get; set; }
    }
}
