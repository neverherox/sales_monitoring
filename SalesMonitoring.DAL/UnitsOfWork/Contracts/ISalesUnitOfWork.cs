using SalesMonitoring.DAL.Repositories.Contracts;
using SalesMonitoring.DML.Models;
using System;

namespace SalesMonitoring.DAL.UnitsOfWork.Contracts
{
    public interface ISalesUnitOfWork : IUnitOfWork
    {
        IGenericRepository<Client> ClientRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        void AddSale(Client client, Product product, DateTime date);
    }
}
