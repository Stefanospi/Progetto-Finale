using E_commerce.Models.ProductCart;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public CartController(ICartService cartService,IAuthService authService)
        {
            _cartService = cartService;
            _authService = authService;
        }


        public async Task<IActionResult> CartProduct()
        {
            var userId = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : (int?)null;
            Cart cart = null;

            if (userId.HasValue)  // Controlla se userId ha un valore
            {
                cart = await _cartService.GetCartByUserIdAsync(userId.Value);  // Usa userId.Value
            }
            else
            {
                string sessionId = Request.Cookies["SessionId"];
                if (sessionId != null)
                {
                    cart = await _cartService.GetCartBySessionIdAsync(sessionId);
                }
            }

            return View(cart?.CartItems ?? new List<CartItems>());
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // Controlla se l'utente è autenticato e recupera l'ID utente se presente
            var userId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : (int?)null;

            if (userId.HasValue)
            {
                // Se l'utente è autenticato, aggiungi al carrello basato sull'utente
                await _cartService.AddToCartAsync(userId.Value, productId, quantity);
            }
            else
            {
                // Se l'utente non è autenticato, gestisci tramite SessionId
                string sessionId = Request.Cookies["SessionId"] ?? Guid.NewGuid().ToString();

                // Se il cookie di sessione non esiste, crea un nuovo sessionId e aggiungilo al cookie
                if (Request.Cookies["SessionId"] == null)
                {
                    Response.Cookies.Append("SessionId", sessionId, new CookieOptions
                    {
                        HttpOnly = true,
                        IsEssential = true,
                        Expires = DateTime.UtcNow.AddDays(30) // Imposta una scadenza di 30 giorni
                    });
                }

                // Controlla se esiste già un carrello per questo sessionId
                var cart = await _cartService.GetCartBySessionIdAsync(sessionId);
                if (cart == null)
                {
                    // Crea un nuovo carrello per la sessione se non esiste
                    await _cartService.CreateCartForSessionAsync(sessionId, productId, quantity);
                }
                else
                {
                    // Aggiungi al carrello esistente
                    await _cartService.AddToCartAsyncForSession(sessionId, productId, quantity);
                }
            }

            return RedirectToAction("CartProduct");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);
            return RedirectToAction("CartProduct");
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            await _cartService.ClearCartAsync(cartId);
            return RedirectToAction("CartProduct");
        }
    }
}
