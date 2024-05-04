using Application.Infrastructure.Data.DbContext;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Infrastructure.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task InsertAndSaveAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<T?> GetSingle(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isCacheable = false, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }

            if (includeProperties.Length > 0)
                query = query.AsSingleQuery();


            return Repository<T>.DoOrderBy(query, orderBy, isCacheable).FirstOrDefaultAsync();
        }

        public Task<T?> GetNotTrackedSingle(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool isCacheable = false, params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }

            if (includeProperties.Length > 0)
                query = query.AsSingleQuery();


            return DoOrderBy(query, orderBy, isCacheable).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetNonTrackedDbSet(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isCacheable = false, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }

            return Repository<T>.DoOrderBy(query, orderBy, isCacheable);
        }

        public IQueryable<T> GetTrackedDbSet(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isCacheable = false, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }

            return Repository<T>.DoOrderBy(query, orderBy, isCacheable);
        }

        private static IQueryable<T> DoOrderBy(IQueryable<T> query, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isCacheable = false)
        {
            if (orderBy != null)
            {
                if (isCacheable)
                    query = orderBy(query).Cacheable(CacheExpirationMode.Sliding, TimeSpan.FromMinutes(10));
                else
                    query = orderBy(query);
            }
            else
            {
                if (isCacheable)
                    query = query.Cacheable(CacheExpirationMode.Sliding, TimeSpan.FromMinutes(10));
            }

            return query;

        }
    }
}
