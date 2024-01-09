using System.Linq.Expressions;

namespace Dashboard.Data.Repository.Base
{
    public interface IBaseRepository<TEntity>
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
    }

}
