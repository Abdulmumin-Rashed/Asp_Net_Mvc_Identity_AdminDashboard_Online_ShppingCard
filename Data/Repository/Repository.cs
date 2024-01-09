using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dashboard.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public void Update2(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
        public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return query.SingleOrDefault(criteria);
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? prediacte = null, string? includeProperities = null)
        {
            IQueryable<T> query = _dbSet;
            if (prediacte != null)
            {
                query = query.Where(prediacte);
            }
            if (includeProperities != null)
            {
                foreach (var item in includeProperities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }

            }
            return query.ToList();
        }

        public T GetT(Expression<Func<T, bool>>? prediacte = null, string? includeProperities = null)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(prediacte);
            if (includeProperities != null)
            {
                foreach (var item in includeProperities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.FirstOrDefault();
        }

    }
}
