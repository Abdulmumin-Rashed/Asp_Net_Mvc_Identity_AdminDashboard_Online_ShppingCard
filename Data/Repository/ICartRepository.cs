using Dashboard.Models;

namespace Dashboard.Data.Repository
{
    public interface ICartRepository: IRepository<Cart>
    {
        Task ClearCartAsync(string userId);
        Task<Cart> GetCartById(int cartId);
        Task<Cart> GetCartByIdAsync(int cartId);
        Task<bool> AddToCartAsync(string userId, int productId, int quantity);
        Task<bool> UpdateQuantityAsync(int cartItemId, int newQuantity);
        Task<bool> DeleteCartItemAsync(int cartItemId);
        Task<List<Cart>> GetCartItemsAsync(string userId);
    }
}
