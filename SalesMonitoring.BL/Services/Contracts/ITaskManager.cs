using System;

namespace SalesMonitoring.BL.Services.Contracts
{
    public interface ITaskManager : IDisposable
    {
        void RegisterWatcherEventHandlers(IDirectoryWatcher watcher);
    }
}
