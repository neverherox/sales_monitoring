using SalesMonitoring.BL.Models;
using SalesMonitoring.BL.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;

namespace SalesMonitoring.BL.Services
{
    public class DirectoryWatcher : IDirectoryWatcher
    {
        private FileSystemWatcher watcher;
        public event EventHandler<FileSystemEventArgs> New;
        private bool _disposed = false;

        public DirectoryWatcher(FileSystemWatcher watcher)
        {
            this.watcher = watcher;
            watcher.Created += OnFileSystemEvent;
        }
        protected void OnFileSystemEvent(object sender, FileSystemEventArgs e)
        {
            New?.Invoke(this, e);
        }
        public void Start()
        {
            watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
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
                New = null;
                watcher.Created -= OnFileSystemEvent;
                watcher.Dispose();
            }
            _disposed = true;
        }

        ~DirectoryWatcher()
        {
            Dispose();
        }
    }
}
