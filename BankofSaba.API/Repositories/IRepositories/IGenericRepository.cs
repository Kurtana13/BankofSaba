using System.Linq.Expressions;

namespace BankofSaba.API.Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>,IOrderedQueryable<T>> orderBy = null,
            string includeProporties = "");

        Task<T> GetByIdAsync(object id);
        Task<T> CreateAsync(T entity);
        T Update(T entity);
        T Delete(T entity);
        Task<IEnumerable<T>> DeleteAsync(Expression<Func<T, bool>> filter = null);
        Task<T> DeleteByIdAsync(object id);
        Task SaveAsync();
    }
}
