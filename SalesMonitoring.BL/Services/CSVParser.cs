using CsvHelper;
using SalesMonitoring.BL.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SalesMonitoring.BL.Services
{
    public class CSVParser : Contracts.IParser
    {
        public IEnumerable<SaleInfoDTO> Parse(string path)
        {
            var sales = new List<SaleInfoDTO>();
            try
            {
                using (var stream = new StreamReader(path))
                {
                    using (var csv = new CsvReader(stream, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        foreach (var info in csv.GetRecords<SaleInfoDTO>())
                        {
                            sales.Add(info);
                        }
                    }
                }
            }
            catch (IOException)
            {
                throw new InvalidOperationException("cannot parse file");
            }
            return sales;
        }
    }
}
