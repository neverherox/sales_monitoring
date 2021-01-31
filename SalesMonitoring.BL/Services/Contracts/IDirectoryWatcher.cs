using System;
using System.IO;

namespace SalesMonitoring.BL.Services.Contracts
{
    public interface IDirectoryWatcher : IProcessHandler, IDisposable
    {
        event EventHandler<FileSystemEventArgs> New;
    }
}
