using System;

namespace SalesMonitoring.DAL.UnitsOfWork.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveContext();
    }
}
