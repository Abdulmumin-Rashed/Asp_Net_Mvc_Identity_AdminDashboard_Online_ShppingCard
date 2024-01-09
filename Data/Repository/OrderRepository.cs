using Dashboard.Data.Repository.Base;
using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<Order>> GetOrdersWithDetailsAsync()
        {
            return await _dbContext.Orders
             .Include(o => o.AppUser)
             .Include(o => o.OrderDetails)
                 .ThenInclude(od => od.Product)
                     .ThenInclude(p => p.Images)
             .ToListAsync();
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            return await _dbContext.Orders
            .Include(o => o.AppUser)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                    .ThenInclude(p => p.Images)
            .SingleOrDefaultAsync(o => o.Id == orderId);    
        }

        public async Task<List<Order>> GetOrdersByUserAsync(string userId)
        {
            return await _dbContext.Orders
                .Include(o => o.AppUser)
                .Where(o => o.AppUserId == userId)
                .ToListAsync();
        }

       
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            _dbContext.Orders.Remove(order);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }


}
