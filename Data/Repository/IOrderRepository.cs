using Dashboard.Data.Repository.Base;
using Dashboard.Models;

namespace Dashboard.Data.Repository
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<List<Order>> GetOrdersWithDetailsAsync();
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        Task<List<Order>> GetOrdersByUserAsync(string userId);
      
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int orderId);
    }


}
