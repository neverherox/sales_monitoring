using SalesMonitoring.BL.Services;
using SalesMonitoring.BL.Services.Contracts;
using System.Configuration;
using System.IO;
using System.ServiceProcess;


namespace SalesMonitoring.ServiceClient
{
    public partial class Service1 : ServiceBase
    {
        private ITaskManager manager;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            IDirectoryWatcher watcher = new DirectoryWatcher(
                   new FileSystemWatcher(ConfigurationManager.AppSettings["sourceFolder"],
                   ConfigurationManager.AppSettings["searchPattern"]));
            manager = new TaskManager(watcher,
                new CSVParser(),
                new CustomTaskScheduler(3),
                new DirectoryHandler(ConfigurationManager.AppSettings["destFolder"]),
                new Logger(ConfigurationManager.AppSettings["logFile"]));
            manager.Start();
        }

        protected override void OnStop()
        {
            manager.Stop();
            manager.Dispose();
        }
    }
}
