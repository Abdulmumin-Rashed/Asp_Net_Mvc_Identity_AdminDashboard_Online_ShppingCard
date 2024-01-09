using Dashboard.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Dashboard.Data.Repository
{
    public class CartRepository : IRepository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddToCartAsync(string userId, int productId, int quantity)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            var cartItem = new Cart
            {
                ProductId = productId,
                AppUserId = userId,
                Product = product,
                AppUser = user,
                Quantity = quantity,
                Total = (decimal)(product.Price * quantity)
            };

            _dbContext.Carts.Add(cartItem);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<Cart> GetCartByIdAsync(int cartId)
        {
            return await _dbContext.Carts
                .Include(c => c.Product)
                    .ThenInclude(p => p.Images) // Include product images
                .Include(c => c.AppUser)
                .SingleOrDefaultAsync(c => c.Id == cartId);
        }
        public async Task<bool> UpdateQuantityAsync(int cartItemId, int newQuantity)
        {
            var cartItem = await _dbContext.Carts
                .Include(ci => ci.Product) // Explicitly load the Product
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem == null)
            {
                return false;
            }

            cartItem.Quantity = newQuantity;
            cartItem.Total = (decimal)(cartItem.Product.Price * newQuantity);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCartItemAsync(int cartItemId)
        {
            var cartItem = await _dbContext.Carts.FindAsync(cartItemId);
            if (cartItem == null)
            {
                return false;
            }

            _dbContext.Carts.Remove(cartItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Cart>> GetCartItemsAsync(string userId)
        {
            return await _dbContext.Carts
                .Include(c => c.Product).ThenInclude(p => p.Images)
                .Where(c => c.AppUserId == userId)
                .ToListAsync();
        }

        public async Task<Cart> GetCartById(int cartId) =>  _dbContext.Carts.Find(cartId);//.Include(c => c.Product)//    .ThenInclude(p => p.Images) // Include product images//.Include(c => c.AppUser)//.SingleOrDefaultAsync(c => c.Id == cartId);

        public IEnumerable<Cart> GetAll(Expression<Func<Cart, bool>>? prediacte = null, string? includeProperities = null)
        {
            throw new NotImplementedException();
        }

        public Cart GetT(Expression<Func<Cart, bool>>? prediacte = null, string? includeProperities = null)
        {
            throw new NotImplementedException();
        }

        public void Add(Cart entity)
        {
            throw new NotImplementedException();
        }

        public Cart Find(Expression<Func<Cart, bool>> criteria, string[] includes = null)
        {
            IQueryable<Cart> query = _dbContext.Set<Cart>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return query.SingleOrDefault(criteria);
        }

        public void Update2(Cart entity)
        {
            _dbContext.Update(entity);
            
        }

        public void Delete(Cart entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<Cart> entity)
        {
            throw new NotImplementedException();
        }

        public async Task ClearCartAsync(string userId)
        {
            var cartItems = await _dbContext.Carts.Where(c => c.AppUserId == userId).ToListAsync();

            _dbContext.Carts.RemoveRange(cartItems);
            await _dbContext.SaveChangesAsync();
        }   
    }


}
