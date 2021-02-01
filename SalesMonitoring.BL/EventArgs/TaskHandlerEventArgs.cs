using SalesMonitoring.BL.Models;
using System.Collections.Generic;

namespace SalesMonitoring.BL.EventArgs
{
    public class TaskHandlerEventArgs : TaskEventArgs
    {
        public string filePath;
        public string fileName;
        public IEnumerable<SaleInfoDTO> sales;
    }
}
