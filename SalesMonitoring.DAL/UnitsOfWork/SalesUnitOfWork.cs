using SalesMonitoring.DAL.Context;
using SalesMonitoring.DAL.Repositories;
using SalesMonitoring.DAL.Repositories.Contracts;
using SalesMonitoring.DAL.UnitsOfWork.Contracts;
using SalesMonitoring.DML.Models;
using System;

namespace SalesMonitoring.DAL.UnitsOfWork
{
    public class SalesUnitOfWork : ISalesUnitOfWork
    {
        private IGenericRepository<Client> clientRepository;
        private IGenericRepository<Order> orderRepository;
        private IGenericRepository<Product> productRepository;
        private SalesContext salesContext;
        public SalesUnitOfWork(SalesContext salesContext)
        {
            this.salesContext = salesContext;
            clientRepository = new GenericRepository<Client>(salesContext);
            orderRepository = new GenericRepository<Order>(salesContext);
            productRepository = new GenericRepository<Product>(salesContext);
        }

        public IGenericRepository<Client> ClientRepository  =>  clientRepository;
        public IGenericRepository<Order> OrderRepository => orderRepository;
        public IGenericRepository<Product> ProductRepository => productRepository;

        public void AddSale(Client client, Product product, DateTime date)
        {
            var foundProduct = productRepository.Get(x => x.Name == product.Name);
            var foundClient = clientRepository.Get(x => x.Name == client.Name);
            Order order = new Order();
            order.Date = date;
            if (foundProduct == null)
            {
                order.Product = product;
            }
            else
            {
                order.Product = foundProduct;
            }
            if (foundClient == null)
            {
                order.Client = client;
            }
            else
            {
                order.Client = foundClient;
            }
            orderRepository.Add(order);
        }
        public void SaveContext()
        {
            salesContext.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    salesContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SalesUnitOfWork()
        {
            Dispose(false);
        }
    }
}
