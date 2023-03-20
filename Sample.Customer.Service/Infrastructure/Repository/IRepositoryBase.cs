using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        #region Declaration

        IEnumerable<T> GetAll();
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> IncludeMultiple(IQueryable<T> query, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate = null, string navigationPropertyPath = "");
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
