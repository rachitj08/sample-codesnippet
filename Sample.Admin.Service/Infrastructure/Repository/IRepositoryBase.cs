using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        #region Declaration

        IEnumerable<T> GetAll();

        IQueryable<T> IncludeMultiple(IQueryable<T> query, params Expression<Func<T, object>>[] includes);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        T Single(Expression<Func<T, bool>> predicate);

        T GetById(long id);

        T Create(T entity);

        T Update(T entity);

        void Delete(T entity);

        void DeleteRange(Expression<Func<T, bool>> predicate);

        int Count(Expression<Func<T, bool>> predicate);

        bool Any(Expression<Func<T, bool>> predicate);

        bool Any();

        #endregion
    }
}
