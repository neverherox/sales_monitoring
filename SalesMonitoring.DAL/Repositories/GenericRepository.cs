using SalesMonitoring.DAL.Context;
using SalesMonitoring.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;


namespace SalesMonitoring.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal SalesContext salesContext;
        internal DbSet<TEntity> entitySet;

        public GenericRepository(SalesContext salesContext)
        {
            this.salesContext = salesContext;
            entitySet = this.salesContext.Set<TEntity>();
        }
        public IEnumerable<TEntity> Get()
        {
            return entitySet.ToList();
        }
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return entitySet.FirstOrDefault(predicate);
        }
        public void Add(TEntity entity)
        {
            if (entity != null)
            {
                entitySet.Add(entity);
            }
        }
        public void Delete(TEntity entity)
        {
            if (entity != null)
            {
                if (salesContext.Entry(entity).State == EntityState.Detached)
                {
                    entitySet.Attach(entity);
                }
                entitySet.Remove(entity);
            }
        }
        public void Update(TEntity entity)
        {
            if (entity != null)
            {
                entitySet.Attach(entity);
                salesContext.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}
