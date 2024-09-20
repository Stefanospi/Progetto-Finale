using E_commerce.Models.ProductCart;

namespace E_commerce.Services.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(int userId, int productId, int quantity, string size);
        Task CreateCartForSessionAsync(string sessionId, int productId, int quantity, string size);
        Task AddToCartAsyncForSession(string sessionId, int productId, int quantity, string size);
        Task<Cart> GetCartBySessionIdAsync(string sessionId);
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<int> GetCartItemCountAsync(int userId);
        Task<int> GetCartItemCountBySessionAsync(string sessionId);

        Task RemoveFromCartAsync(int cartItemId);
        Task ClearCartAsync(int cartId);
        Task ClearCartNoLogginAsync(string sessionId);

    }



}
