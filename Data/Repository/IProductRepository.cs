using Dashboard.Models;

namespace Dashboard.Data.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
        void Add2(Product product);
        IEnumerable<Product> GetAllProductsWithDetails();
        Product GetOne(int? id);
    }
}
