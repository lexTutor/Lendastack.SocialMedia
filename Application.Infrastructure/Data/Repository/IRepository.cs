
using System.Linq.Expressions;

namespace Application.Infrastructure.Data.Repository
{
    public interface IRepository<T>
    {
        void Delete(T entity);
        IQueryable<T> GetNonTrackedDbSet(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isCacheable = false, params Expression<Func<T, object>>[] includeProperties);
        Task<T?> GetNotTrackedSingle(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool isCacheable = false, params Expression<Func<T, object>>[] includeProperties);
        Task<T?> GetSingle(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isCacheable = false, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetTrackedDbSet(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isCacheable = false, params Expression<Func<T, object>>[] includeProperties);
        Task InsertAndSaveAsync(T entity, CancellationToken cancellationToken);
        Task Update(T entity, CancellationToken cancellationToken = default);
    }
}