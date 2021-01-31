using System;

namespace SalesMonitoring.BL.Services.Contracts
{
    public interface ILogger : IDisposable
    {
        void LogInfo(string message);
    }
}
