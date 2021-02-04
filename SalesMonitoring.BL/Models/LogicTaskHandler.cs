using SalesMonitoring.BL.EventArgs;
using SalesMonitoring.BL.Services;
using SalesMonitoring.BL.Services.Contracts;
using SalesMonitoring.DAL.Context;
using SalesMonitoring.DAL.UnitsOfWork;
using SalesMonitoring.DML.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace SalesMonitoring.BL.Models
{
    public class LogicTaskHandler : LogicTask
    {
        private ILogger logger;
        private static object locker = new object();
        public LogicTaskHandler()
        {
            BeforeExecuting += ParseCSV;
            Executing += AddSale;
            AfterExecuting += BackUpFile;
            logger = new Logger(ConfigurationManager.AppSettings["logFile"]);
        }

        private void ParseCSV(object sender, TaskEventArgs arg)
        {
            var handlerArg = arg as TaskHandlerEventArgs;
            var parser = new CSVParser();
            handlerArg.sales = parser.Parse(handlerArg.filePath + handlerArg.fileName);
            logger.LogInfo(Task.CurrentId + " parsed file " + handlerArg.fileName);
        }
        private void AddSale(object sender, TaskEventArgs arg)
        {
            var handlerArg = arg as TaskHandlerEventArgs;
            foreach (var sale in handlerArg.sales)
            {
                var context = new SalesContext();
                var uow = new SalesUnitOfWork(context);
                lock (locker)
                {
                    try
                    {
                        uow.AddSale
                        (
                            new Client { Name = sale.CustomerName },
                            new Product { Name = sale.ProductName, Price = sale.ProductPrice },
                            sale.Date
                         );
                        uow.SaveContext();
                        logger.LogInfo(Task.CurrentId + " added sale " + sale.ToString());
                    }
                    catch (Exception)
                    {
                        throw new InvalidOperationException(Task.CurrentId + " failed adding " + sale.ToString() + " from " + handlerArg.fileName);
                    }
                    finally
                    {
                        uow.Dispose();
                    }
                }
            }

        }

        private void BackUpFile(object sender, TaskEventArgs arg)
        {
            var handlerArg = arg as TaskHandlerEventArgs;
            var handler = new DirectoryHandler(ConfigurationManager.AppSettings["processedFolder"]);
            handler.BackUp(handlerArg.filePath, handlerArg.fileName);
            logger.LogInfo(Task.CurrentId + " back upped file " + handlerArg.fileName);
        }
    }
}
