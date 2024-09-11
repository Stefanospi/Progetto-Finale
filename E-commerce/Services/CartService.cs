using E_commerce.Context;
using E_commerce.Models.ProductCart;
using E_commerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Services
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;

        public CartService(DataContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await _context.Cart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Cart.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItems { ProductId = productId, Quantity = quantity });
            }

            await _context.SaveChangesAsync();
        }

        public async Task CreateCartForSessionAsync(string sessionId, int productId, int quantity)
        {
            var cart = new Cart { SessionId = sessionId };
            _context.Cart.Add(cart);

            cart.CartItems.Add(new CartItems { ProductId = productId, Quantity = quantity });

            await _context.SaveChangesAsync();
        }

        public async Task AddToCartAsyncForSession(string sessionId, int productId, int quantity)
        {
            var cart = await _context.Cart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);

            if (cart == null)
            {
                throw new InvalidOperationException("Il carrello per questa sessione non esiste.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItems { ProductId = productId, Quantity = quantity });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartBySessionIdAsync(string sessionId)
        {
            return await _context.Cart
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Cart
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int cartId)
        {
            var cart = await _context.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);  // Rimuove tutti i prodotti dal carrello
                _context.Cart.Remove(cart);  // Rimuove il carrello stesso
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartNoLogginAsync(string sessionId)
        {
            var cart = await _context.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.SessionId == sessionId);
            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);  // Rimuove i prodotti associati
                _context.Cart.Remove(cart);  // Rimuove il carrello
                await _context.SaveChangesAsync();
            }
        }
    }
}
