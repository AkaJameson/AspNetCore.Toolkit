using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TQ_EntityFramework.UnitofWork.UnitofWork.Abstractions;

namespace TQ_EntityFramework.UnitofWork.UnitofWork
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> DbSet;
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public IQueryable<T> AsNoTracking()
        {
            return DbSet.AsNoTracking(); 
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if(predicate == null)
            {
                return await DbSet.CountAsync();
            }
            return await DbSet.CountAsync(predicate);
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            DbSet.Remove(entity);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = await DbSet.Where(predicate).ToListAsync();
            if (entities.Any())
            {
                DbSet.RemoveRange(entities);
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>> orderBy = null,
            bool ascending = true)
        {
            var query = DbSet.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var totalCount = await query.CountAsync();

            if (orderBy != null)
            {
                query = ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }

            var items = await query
                        .Skip(Math.Max(0, pageIndex - 1) * Math.Max(1, pageSize))
                        .Take(Math.Max(1, pageSize))
                        .ToListAsync();

            return (items, totalCount);
        }
        public async Task<(IEnumerable<TResult> Items, int TotalCount)> GetPagedAsync<TResult>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Expression<Func<T, TResult>> selector = null)
        {
            var query = DbSet.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var totalCount = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query
                .Skip(Math.Max(0, pageIndex - 1) * Math.Max(1, pageSize))
                .Take(Math.Max(1, pageSize));

            if (selector != null)
            {
                var projected = query.Select(selector);
                return (await projected.ToListAsync(), totalCount);
            }
            else
            {
                var defaultProjected = query.Cast<T>().Select(e => (TResult)(object)e);
                return (await defaultProjected.ToListAsync(), totalCount);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            DbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                DbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }


        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
    }
}
