﻿using SalesMonitoring.BL.EventArgs;
using SalesMonitoring.BL.Services;
using System;
using System.Configuration;
using System.Transactions;

namespace SalesMonitoring.BL.Models
{
    public abstract class LogicTask : IDisposable
    {
        public event EventHandler<TaskEventArgs> BeforeExecuting;
        public event EventHandler<TaskEventArgs> Executing;
        public event EventHandler<TaskEventArgs> AfterExecuting;
        private bool _disposed = false;

        public virtual void Execute(TaskEventArgs arg)
        {
            var logger = new Logger(ConfigurationManager.AppSettings["logFile"]);
            try
            { 
                BeforeExecuting?.Invoke(this, arg);
                Executing?.Invoke(this, arg);
                AfterExecuting?.Invoke(this, arg);
            }
            catch(InvalidOperationException exc)
            {
                logger.LogInfo(exc.Message);
            }
        }
        public void ClearEvents()
        {
            BeforeExecuting = null;
            Executing = null;
            AfterExecuting = null;
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
                ClearEvents();
            }
            _disposed = true;
        }
        ~LogicTask()
        {
            Dispose();
        }
    }
}
