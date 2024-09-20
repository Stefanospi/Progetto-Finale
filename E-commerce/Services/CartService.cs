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

        public async Task AddToCartAsync(int userId, int productId, int quantity, string size)
        {
            if (quantity <= 0) throw new ArgumentException("Quantità non valida.");
            if (userId <= 0) throw new ArgumentException("ID utente non valido.");

            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new ArgumentException("Prodotto non valido.");

            var cart = await _context.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Cart.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId && ci.Size == size);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItems { ProductId = productId, Quantity = quantity, Size = size });
            }

            await _context.SaveChangesAsync();
        }
        public async Task CreateCartForSessionAsync(string sessionId, int productId, int quantity, string size)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("SessionId non valido.");
            if (quantity <= 0) throw new ArgumentException("Quantità non valida.");

            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new ArgumentException("Prodotto non valido.");

            var cart = new Cart { SessionId = sessionId };
            _context.Cart.Add(cart);

            cart.CartItems.Add(new CartItems { ProductId = productId, Quantity = quantity, Size = size });

            await _context.SaveChangesAsync();
        }
        public async Task AddToCartAsyncForSession(string sessionId, int productId, int quantity, string size)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("SessionId non valido.");
            if (quantity <= 0) throw new ArgumentException("Quantità non valida.");

            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new ArgumentException("Prodotto non valido.");

            var cart = await _context.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.SessionId == sessionId);
            if (cart == null)
            {
                cart = new Cart { SessionId = sessionId };
                _context.Cart.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId && ci.Size == size);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItems { ProductId = productId, Quantity = quantity, Size = size });
            }

            await _context.SaveChangesAsync();
        }
        public async Task<Cart> GetCartBySessionIdAsync(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("SessionId non valido.");
            return await _context.Cart
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("ID utente non valido.");
            return await _context.Cart
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            if (cartItemId <= 0) throw new ArgumentException("ID carrello non valido.");

            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) throw new ArgumentException("Elemento del carrello non trovato.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int cartId)
        {
            if (cartId <= 0) throw new ArgumentException("ID carrello non valido.");

            var cart = await _context.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart == null) throw new ArgumentException("Carrello non trovato.");

            _context.CartItems.RemoveRange(cart.CartItems);  // Rimuove tutti i prodotti dal carrello
            _context.Cart.Remove(cart);  // Rimuove il carrello stesso
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartNoLogginAsync(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("SessionId non valido.");

            var cart = await _context.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.SessionId == sessionId);
            if (cart == null) throw new ArgumentException("Carrello non trovato.");

            _context.CartItems.RemoveRange(cart.CartItems);  // Rimuove i prodotti associati
            _context.Cart.Remove(cart);  // Rimuove il carrello
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCartItemCountAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("ID utente non valido.");

            var cart = await _context.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
        }

        public async Task<int> GetCartItemCountBySessionAsync(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("SessionId non valido.");

            var cart = await _context.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.SessionId == sessionId);
            return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
        }
    }
}
