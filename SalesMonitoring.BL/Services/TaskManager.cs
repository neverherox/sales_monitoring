using SalesMonitoring.BL.Services.Contracts;
using SalesMonitoring.DAL.Context;
using SalesMonitoring.DAL.UnitsOfWork;
using SalesMonitoring.DML.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SalesMonitoring.BL.Services
{
    public class TaskManager : ITaskManager
    {
        private IDirectoryWatcher watcher;
        private IParser parser;
        private CustomTaskScheduler scheduler;
        private IDirectoryHandler handler;
        private ILogger logger;

        private CancellationTokenSource tokenSource;
        private Object locker = new Object();
        private bool _disposed = false;
        public TaskManager(IDirectoryWatcher watcher, IParser parser, CustomTaskScheduler scheduler, IDirectoryHandler handler, ILogger logger)
        {
            using (var salesContext = new SalesContext())
            {
                salesContext.Database.CreateIfNotExists();
            }

            tokenSource = new CancellationTokenSource();

            this.watcher = watcher;
            this.parser = parser;
            this.scheduler = scheduler;
            this.handler = handler;
            this.logger = logger;
            watcher.New += RunTask;
        }
        private void RunTask(object sender, FileSystemEventArgs e)
        {
            var task = new Task(() =>
            {
                logger.LogInfo(Task.CurrentId + " is started");
                var sales = parser.Parse(e.FullPath);
                logger.LogInfo(Task.CurrentId + " parsed file");
                foreach (var sale in sales)
                {
                    lock (locker)
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
                            logger.LogInfo(Task.CurrentId + " is added to db " + sale.ToString() + " from " + e.Name);
                        }
                        catch (Exception exc)
                        {
                            throw new InvalidOperationException("cannot add info " + sale.ToString() + exc.Message, exc);
                        }
                        finally
                        {
                            uow.Dispose();
                            context.Dispose();
                        }
                    }
                }
                handler.BackUp(e.FullPath.Remove(e.FullPath.IndexOf(e.Name), e.Name.Length), e.Name);
                logger.LogInfo(Task.CurrentId + " is ended");

            }, tokenSource.Token);
            task.Start(scheduler);
        }
        public void Start()
        {
            watcher.Start();
        }

        public void Stop()
        {
            watcher.Stop();
            tokenSource.Cancel();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                logger.Dispose();
                watcher.Dispose();
                tokenSource.Dispose();
            }
            _disposed = true;
        }
        ~TaskManager()
        {
            Dispose();
        }
    }
}
