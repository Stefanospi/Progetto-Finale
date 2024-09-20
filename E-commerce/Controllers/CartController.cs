using E_commerce.Models.ProductCart;
using E_commerce.Services.Helper;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly CartHelper _cartHelper;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IAuthService authService, CartHelper cartHelper, IProductService productService)
        {
            _cartService = cartService;
            _authService = authService;
            _cartHelper = cartHelper;
            _productService = productService;
        }

        public async Task<IActionResult> GetCartItemCount()
        {
            // Usa CartHelper per aggiornare il conteggio degli articoli nel carrello
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);
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

            // Usa CartHelper per aggiornare il conteggio degli articoli nel carrello
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);

            return View(cart?.CartItems ?? new List<CartItems>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity, string size)
        {
            var userId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : (int?)null;

            if (userId.HasValue)
            {
                await _cartService.AddToCartAsync(userId.Value, productId, quantity, size);
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
                    await _cartService.CreateCartForSessionAsync(sessionId, productId, quantity, size);
                }
                else
                {
                    await _cartService.AddToCartAsyncForSession(sessionId, productId, quantity, size);
                }
            }

            // Dopo aver aggiunto al carrello, sottrai la quantità acquistata dallo stock
            await _productService.DecreaseProductStockAsync(productId, quantity);

            return RedirectToAction("CartProduct");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);

            // Usa CartHelper per aggiornare il conteggio degli articoli nel carrello dopo la rimozione
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);

            return RedirectToAction("CartProduct");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            await _cartService.ClearCartAsync(cartId);

            // Usa CartHelper per aggiornare il conteggio degli articoli nel carrello dopo la pulizia
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);

            return RedirectToAction("CartProduct");
        }
    }
}
