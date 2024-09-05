﻿using E_commerce.Models.ProductCart;

namespace E_commerce.Services.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(int userId, int productId, int quantity);
        Task CreateCartForSessionAsync(string sessionId, int productId, int quantity);
        Task AddToCartAsyncForSession(string sessionId, int productId, int quantity);
        Task<Cart> GetCartBySessionIdAsync(string sessionId);
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task RemoveFromCartAsync(int cartItemId);
        Task ClearCartAsync(int cartId);
    }



}
