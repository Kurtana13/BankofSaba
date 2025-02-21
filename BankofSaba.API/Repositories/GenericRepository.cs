using BankofSaba.API.Data;
using BankofSaba.API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BankofSaba.API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string includeProporties = "")
        {
            IQueryable<T> query = _dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProporties))
            {
                foreach(var includeProperty in includeProporties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task CreateAsync(T entity)
        {
            if(entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            if(entity == null)
                throw new ArgumentNullException(nameof(entity));
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            if(entity == null)
                throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> DeleteAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            var entitiesToDelete = await query.ToListAsync();

            _dbSet.RemoveRange(entitiesToDelete);


            return entitiesToDelete;
        }

        public async Task DeleteByIdAsync(object id)
        {
            var entity = await GetByIdAsync(id);

            if(entity != null)
            {
                Delete(entity);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
