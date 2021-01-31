using SalesMonitoring.BL.Services;
using SalesMonitoring.BL.Services.Contracts;
using System;
using System.Collections.Concurrent;
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
            ITaskManager manager = new TaskManager(watcher,
                new CSVParser(),
                new CustomTaskScheduler(3),
                new DirectoryHandler(ConfigurationManager.AppSettings["destFolder"]),
                new Logger(ConfigurationManager.AppSettings["logFile"]));
            manager.Start();
            Console.ReadKey();
            manager.Stop();
            manager.Dispose();
            Console.ReadKey();
        }
    }
}
