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
        private IDirectoryWatcher watcher;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            watcher = new DirectoryWatcher(
                  new FileSystemWatcher(ConfigurationManager.AppSettings["sourceFolder"],
                  ConfigurationManager.AppSettings["searchPattern"]));
            manager = new TaskManager(new CustomTaskScheduler(3));
            manager.RegisterWatcherEventHandlers(watcher);
            watcher.Start();
        }

        protected override void OnStop()
        {
            watcher.Stop();
            manager.Dispose();
            watcher.Dispose();
        }
    }
}
