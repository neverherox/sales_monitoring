using System;

namespace SalesMonitoring.BL.Models
{
    public class SaleInfoDTO
    { 
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }

        public override string ToString()
        {
            return Date + " " + CustomerName + " " + ProductName + " " + ProductPrice;
        }
    }
}
