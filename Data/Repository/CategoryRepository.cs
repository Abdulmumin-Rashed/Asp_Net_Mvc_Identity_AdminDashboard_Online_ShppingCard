using Dashboard.Models;
using System.Collections;

namespace Dashboard.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable ToList()
        {
            return _context.Categories.ToList();
        }

        public void Update(Category category)
        {
            var categoryDB = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (categoryDB != null)
            {
                categoryDB.Name = category.Name;
                category.Status =  category.Status;
                
            }
        }
    }
}
