using SalesMonitoring.BL.Services;
using SalesMonitoring.BL.Services.Contracts;
using System;
using System.Configuration;
using System.IO;

namespace SalesMonitoring.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IDirectoryWatcher watcher = new DirectoryWatcher(
                    new FileSystemWatcher(ConfigurationManager.AppSettings["sourceFolder"],
                    ConfigurationManager.AppSettings["searchPattern"]));
            ITaskManager manager = new TaskManager(2);
            manager.RegisterWatcherEventHandlers(watcher);
            watcher.Start();
            Console.ReadKey();
            watcher.Stop();
            manager.Dispose();
            watcher.Dispose();
        }
    }
}
