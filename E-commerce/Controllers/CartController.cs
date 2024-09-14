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

        public CartController(ICartService cartService, IAuthService authService)
        {
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<IActionResult> GetCartItemCount()
        {
            await UpdateCartItemCount();
            return PartialView("_CartItemCountPartial", ViewBag.CartItemCount);
        }

        public async Task<IActionResult> CartProduct()
        {
            var userId = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : (int?)null;
            Cart cart = null;

            if (userId.HasValue)
            {
                cart = await _cartService.GetCartByUserIdAsync(userId.Value);
            }
            else
            {
                string sessionId = Request.Cookies["SessionId"];
                if (sessionId != null)
                {
                    cart = await _cartService.GetCartBySessionIdAsync(sessionId);
                }
            }

            // Aggiorna il conteggio degli articoli nel carrello
            await UpdateCartItemCount();

            return View(cart?.CartItems ?? new List<CartItems>());
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : (int?)null;

            if (userId.HasValue)
            {
                await _cartService.AddToCartAsync(userId.Value, productId, quantity);
            }
            else
            {
                string sessionId = Request.Cookies["SessionId"] ?? Guid.NewGuid().ToString();

                if (Request.Cookies["SessionId"] == null)
                {
                    Response.Cookies.Append("SessionId", sessionId, new CookieOptions
                    {
                        HttpOnly = true,
                        IsEssential = true,
                        Expires = DateTime.UtcNow.AddDays(30)
                    });
                }

                var cart = await _cartService.GetCartBySessionIdAsync(sessionId);
                if (cart == null)
                {
                    await _cartService.CreateCartForSessionAsync(sessionId, productId, quantity);
                }
                else
                {
                    await _cartService.AddToCartAsyncForSession(sessionId, productId, quantity);
                }
            }

            // Aggiorna il conteggio degli articoli nel carrello dopo l'aggiunta
            await UpdateCartItemCount();

            return RedirectToAction("CartProduct");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);

            // Aggiorna il conteggio degli articoli nel carrello dopo la rimozione
            await UpdateCartItemCount();

            return RedirectToAction("CartProduct");
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            await _cartService.ClearCartAsync(cartId);

            // Aggiorna il conteggio degli articoli nel carrello dopo la pulizia
            await UpdateCartItemCount();

            return RedirectToAction("CartProduct");
        }

        private async Task UpdateCartItemCount()
        {
            int itemCount = 0;

            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                itemCount = await _cartService.GetCartItemCountAsync(userId);
            }
            else
            {
                var sessionId = Request.Cookies["SessionId"];
                if (sessionId != null)
                {
                    itemCount = await _cartService.GetCartItemCountBySessionAsync(sessionId);
                }
            }

            ViewBag.CartItemCount = itemCount;
        }
    }
}
