using SalesMonitoring.BL.EventArgs;
using SalesMonitoring.BL.Models;
using SalesMonitoring.BL.Services.Contracts;
using SalesMonitoring.DAL.Context;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SalesMonitoring.BL.Services
{
    public class TaskManager : ITaskManager
    {
        private CancellationTokenSource tokenSource;
        private SemaphoreSlim concurrencySemaphore;
        private IProducerConsumerCollection<Task> runningTasks;
        private bool _disposed = false;
        public TaskManager(int maxConcurrency)
        {
            using (var salesContext = new SalesContext())
            {
                salesContext.Database.CreateIfNotExists();
            }
            runningTasks = new ConcurrentStack<Task>();
            tokenSource = new CancellationTokenSource();
            concurrencySemaphore = new SemaphoreSlim(maxConcurrency);
        }

        public virtual void RegisterWatcherEventHandlers(IDirectoryWatcher watcher)
        {
            watcher.New += RunLogicTask;
            watcher.Stopping += CancelTasks;
        }
        private void RunLogicTask(object sender, FileSystemEventArgs e)
        {
            Task task = null;
            task = Task.Run(() =>
            {
                runningTasks.TryAdd(task);
                concurrencySemaphore.Wait();
                using (LogicTask logicTaskHandler = new LogicTaskHandler())
                {
                    logicTaskHandler.Execute
                    (new TaskHandlerEventArgs
                    {
                        fileName = e.Name,
                        filePath = e.FullPath.Remove(e.FullPath.IndexOf(e.Name), e.Name.Length),
                    });
                }
                concurrencySemaphore.Release();
                runningTasks.TryTake(out task);
            }, tokenSource.Token);
        }
        private void CancelTasks(object sender, System.EventArgs e)
        {
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
                Task.WaitAll(runningTasks.ToArray());
                tokenSource.Dispose();
                concurrencySemaphore.Dispose();
            }
            _disposed = true;
        }
        ~TaskManager()
        {
            Dispose();
        }
    }
}
