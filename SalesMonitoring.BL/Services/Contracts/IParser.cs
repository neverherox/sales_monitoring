using SalesMonitoring.BL.Models;
using System.Collections.Generic;

namespace SalesMonitoring.BL.Services.Contracts
{
    public interface IParser
    {
        IEnumerable<SaleInfoDTO> Parse(string path);
    }
}
