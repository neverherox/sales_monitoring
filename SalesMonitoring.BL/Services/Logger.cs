using SalesMonitoring.BL.Services.Contracts;
using System;
using System.IO;

namespace SalesMonitoring.BL.Services
{
    public class Logger : ILogger
    {
        private string filePath;
        private static StreamWriter writer;
        private static object locker = new object();

        public Logger(string filePath)
        {
            this.filePath = filePath;
        }
        public void LogInfo(string message)
        {
            lock (locker)
            {
                try
                {
                    if (writer == null)
                    {
                        writer = new StreamWriter(filePath);
                    }
                    else
                    {
                        writer = File.AppendText(filePath);
                    }
                    writer.WriteLine(message);
                }
                catch (IOException)
                {
                    throw new InvalidOperationException("cannot log message");
                }
                finally
                {
                    writer.Close();
                    writer.Dispose();
                }    
            }
        }
    }
}
