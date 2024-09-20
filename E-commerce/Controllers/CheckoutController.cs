using E_commerce.Models.Auth;
using E_commerce.Models.Order;
using E_commerce.Models.ProductCart;
using E_commerce.Services.Helper;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAddressService _addressService;
        private readonly IOrderService _orderService;
        private readonly CartHelper _cartHelper;  // Aggiungi CartHelper

        public CheckoutController(ICartService cartService, IAddressService addressService, IOrderService orderService, CartHelper cartHelper)
        {
            _cartService = cartService;
            _addressService = addressService;
            _orderService = orderService;
            _cartHelper = cartHelper;  // Inizializza CartHelper
        }

        // Passaggio 1: Mostra il form per l'aggiunta di un nuovo indirizzo o reindirizza alla selezione
        public async Task<IActionResult> AddAddress(bool forceAdd = false)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var existingAddresses = await _addressService.GetAddressesByUserIdAsync(userId);

                if (existingAddresses.Any() && !forceAdd)
                {
                    return RedirectToAction("SelectAddress");
                }
            }
            else
            {
                string sessionId = Request.Cookies["SessionId"];
                if (string.IsNullOrEmpty(sessionId))
                {
                    sessionId = Guid.NewGuid().ToString();
                    Response.Cookies.Append("SessionId", sessionId, new CookieOptions { Expires = DateTime.Now.AddMonths(1) });
                }

                var existingAddresses = await _addressService.GetAddressesBySessionIdAsync(sessionId);
                if (existingAddresses.Any() && !forceAdd)
                {
                    return RedirectToAction("SelectAddress");
                }
            }

            // Aggiorna il conteggio degli articoli nel carrello
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(Addresses address)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                address.UserId = userId;
            }
            else
            {
                string sessionId = Request.Cookies["SessionId"];
                if (string.IsNullOrEmpty(sessionId))
                {
                    sessionId = Guid.NewGuid().ToString();
                    Response.Cookies.Append("SessionId", sessionId, new CookieOptions { Expires = DateTime.Now.AddMonths(1) });
                }
                address.SessionId = sessionId;
            }

            await _addressService.AddAddresses(address);

            // Aggiorna il conteggio degli articoli nel carrello
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);

            return RedirectToAction("SelectAddress");
        }

        // Passaggio 2: Mostra gli indirizzi salvati per la selezione
        public async Task<IActionResult> SelectAddress()
        {
            IEnumerable<Addresses> addresses;

            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                addresses = await _addressService.GetAddressesByUserIdAsync(userId);
            }
            else
            {
                string sessionId = Request.Cookies["SessionId"];
                if (string.IsNullOrEmpty(sessionId))
                {
                    return RedirectToAction("AddAddress");
                }
                addresses = await _addressService.GetAddressesBySessionIdAsync(sessionId);
            }

            // Aggiorna il conteggio degli articoli nel carrello
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);

            return View(addresses);
        }

        // Passaggio 2: Creazione dell'ordine prima del pagamento
        [HttpPost]
        public async Task<IActionResult> CreateOrder(int addressId)
        {
            int? userId = null;
            string sessionId = null;
            Cart cart = null;

            if (User.Identity.IsAuthenticated)
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                cart = await _cartService.GetCartByUserIdAsync(userId.Value);
            }
            else
            {
                sessionId = Request.Cookies["SessionId"];
                if (string.IsNullOrEmpty(sessionId))
                {
                    sessionId = Guid.NewGuid().ToString();
                    Response.Cookies.Append("SessionId", sessionId, new CookieOptions { Expires = DateTime.Now.AddMonths(1) });
                }
                cart = await _cartService.GetCartBySessionIdAsync(sessionId);
            }

            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest("Il carrello è vuoto.");
            }

            decimal totalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            var order = new Orders
            {
                UserId = userId,
                SessionId = sessionId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending",
                ShippingAddressId = addressId,
                OrderItems = cart.CartItems.Select(ci => new OrderItems
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                }).ToList()
            };

            await _orderService.CreateOrderAsync(order);

            // Aggiorna il conteggio degli articoli nel carrello
            ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);

            return RedirectToAction("Checkout", "Payment", new { orderId = order.OrderId });
        }
    }
}
