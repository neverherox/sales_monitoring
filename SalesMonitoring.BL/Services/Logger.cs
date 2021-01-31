using SalesMonitoring.BL.Services.Contracts;
using System;
using System.IO;

namespace SalesMonitoring.BL.Services
{
    public class Logger : ILogger
    {
        string filePath;
        private bool _disposed = false;
        private StreamWriter writer;
        public Logger(string filePath)
        {
            this.filePath = filePath;
            writer = new StreamWriter(this.filePath, false);
        }
        public void LogInfo(string message)
        {
            lock (writer)
            {
                try
                {
                    writer.WriteLine("Date: " + DateTime.Now.ToString() + " Log info: " + message);
                }
                catch (IOException e)
                {
                    throw new InvalidOperationException("cannot log message", e);
                }
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
                writer.Close();
                writer.Dispose();
            }
            _disposed = true;
        }
        ~Logger()
        {
            Dispose();
        }
    }
}
