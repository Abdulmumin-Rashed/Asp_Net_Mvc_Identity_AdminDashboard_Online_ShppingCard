using Dashboard.Models;
using System.Collections;

namespace Dashboard.Data.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
        IEnumerable ToList();
    }
}
