using SalesMonitoring.BL.EventArgs;
using SalesMonitoring.BL.Models;
using SalesMonitoring.BL.Services.Contracts;
using SalesMonitoring.DAL.Context;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SalesMonitoring.BL.Services
{
    public class TaskManager : ITaskManager
    {
        private CustomTaskScheduler scheduler;
        private CancellationTokenSource tokenSource;
        private bool _disposed = false;
        public TaskManager(CustomTaskScheduler scheduler)
        {
            using (var salesContext = new SalesContext())
            {
                salesContext.Database.CreateIfNotExists();
            }
            tokenSource = new CancellationTokenSource();
            this.scheduler = scheduler;
        }

        public virtual void RegisterWatcherEventHandlers(IDirectoryWatcher watcher)
        {
            watcher.New += RunLogicTask;
            watcher.Stopping += CancelTasks;
        }
        private void RunLogicTask(object sender, FileSystemEventArgs e)
        {
            var task = new Task(() =>
            {
                LogicTask logicTaskHandler = new LogicTaskHandler(
                   new CSVParser(),
                   new DirectoryHandler(ConfigurationManager.AppSettings["destFolder"]),
                   new Logger(ConfigurationManager.AppSettings["logFile"]));
                try
                {
                    logicTaskHandler.Execute(new TaskHandlerEventArgs
                    {
                        filePath = e.FullPath,
                        fileName = e.Name
                    });
                }
                finally
                {
                    logicTaskHandler.Dispose();
                }
            }, tokenSource.Token);
            task.Start(scheduler);
        }
        private void CancelTasks(object sender, System.EventArgs e)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }
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
