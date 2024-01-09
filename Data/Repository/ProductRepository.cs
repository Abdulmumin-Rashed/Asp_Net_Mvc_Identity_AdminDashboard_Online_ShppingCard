using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var productDB = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (productDB != null)
            {
                productDB.Name = product.Name;
                productDB.Description = product.Description;
                productDB.Price = product.Price;
               productDB.CategoryId = product.CategoryId;
            }
        }

        public void Add2(Product product)
        {


           
            Add(product);
            
        }

        public IEnumerable<Product> GetAllProductsWithDetails()
        {
            return _context.Products
            .Include(p => p.Category) // Include Category navigation property
            .Include(p => p.Images)   // Include Images navigation property
            
            .ToList();


        }

        public Product GetOne(int? id)
        {
            return _context.Products
           .Include(p => p.Category)
           .Include(p => p.Images)
           .FirstOrDefault(p => p.Id == id);
        }
    }
}
