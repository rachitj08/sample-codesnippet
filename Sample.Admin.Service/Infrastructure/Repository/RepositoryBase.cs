using Sample.Admin.Service.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        #region Properties

        public readonly CloudAcceleratorContext context;

        public RepositoryBase(CloudAcceleratorContext context)
        {
            this.context = context;
        }

        protected void Save() => this.context.SaveChanges();

        public T Create(T entity)
        {
            this.context.Add(entity);
            return entity;
        }

        public virtual void Delete(T entity)
        {
            this.context.Remove(entity);
        }

        public void DeleteRange(Expression<Func<T, bool>> predicate)
        {
            this.context.RemoveRange(context.Set<T>().Where(predicate));
        }

        public IEnumerable<T> GetAll()
        {
            return this.context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> IncludeMultiple(IQueryable<T> query, params Expression<Func<T, object>>[] includes)
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return query;
        }

        public T GetById(long id)
        {
            return this.context.Set<T>().Find(id);
        }

        public T Update(T entity)
        {
            this.context.Update(entity);
            return entity;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this.context.Set<T>().Where(predicate).ToList();
        }

        public IQueryable<T> FindWithSelectedColumns(Expression<Func<T, bool>> predicate)
        {
            return this.context.Set<T>().Where(predicate);
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return this.context.Set<T>().Where(predicate).FirstOrDefault();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return this.context.Set<T>().Count(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return this.context.Set<T>().Any(predicate);
        }

        public bool Any()
        {
            return this.context.Set<T>().Any();
        }

        #endregion

    }
}
