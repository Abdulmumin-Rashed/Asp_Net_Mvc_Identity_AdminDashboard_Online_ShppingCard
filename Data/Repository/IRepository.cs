using System.Linq.Expressions;

namespace Dashboard.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? prediacte = null, string? includeProperities = null);
        T GetT(Expression<Func<T, bool>>? prediacte = null, string? includeProperities = null);
        void Add(T entity);
        T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        void Update2(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entity);
    }
}
