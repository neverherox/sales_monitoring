using SalesMonitoring.BL.EventArgs;
using SalesMonitoring.BL.Services.Contracts;
using SalesMonitoring.DAL.Context;
using SalesMonitoring.DAL.UnitsOfWork;
using SalesMonitoring.DML.Models;
using System;
using System.Threading.Tasks;

namespace SalesMonitoring.BL.Models
{
    public class LogicTaskHandler : LogicTask
    {
        private IParser parser;
        private IDirectoryHandler handler;
        private ILogger logger;

        public LogicTaskHandler(IParser parser, IDirectoryHandler handler, ILogger logger)
        {
            BeforeExecuting += ParseCSV;
            Executing += AddSale;
            AfterExecuting += BackUpFile;

            this.parser = parser;
            this.handler = handler;
            this.logger = logger;
        }

        private void ParseCSV(object sender, TaskEventArgs arg)
        {
            var handlerArg = arg as TaskHandlerEventArgs;
            handlerArg.sales = parser.Parse(handlerArg.filePath);
            logger.LogInfo(Task.CurrentId + " parsed file " + handlerArg.fileName);
        }
        private void AddSale(object sender, TaskEventArgs arg)
        {
            var handlerArg = arg as TaskHandlerEventArgs;
            foreach (var sale in handlerArg.sales)
            {
                var context = new SalesContext();
                var uow = new SalesUnitOfWork(context);
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
                    logger.LogInfo(Task.CurrentId + " failed adding sale " + sale.ToString());
                }
                finally
                {
                    uow.Dispose();
                    context.Dispose();
                }
            }
        }
        private void BackUpFile(object sender, TaskEventArgs arg)
        {
            var handlerArg = arg as TaskHandlerEventArgs;
            handler.BackUp(handlerArg.filePath.Remove(handlerArg.filePath.IndexOf(handlerArg.fileName), handlerArg.fileName.Length), handlerArg.fileName);
            logger.LogInfo(Task.CurrentId + " back upped file " + handlerArg.fileName);
        }
    }
}
