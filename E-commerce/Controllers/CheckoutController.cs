using E_commerce.Models.Auth;
using E_commerce.Models.Order;
using E_commerce.Models.ProductCart;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAddressService _addressService;
        private readonly IOrderService _orderService;

        public CheckoutController(ICartService cartService, IAddressService addressService, IOrderService orderService)
        {
            _cartService = cartService;
            _addressService = addressService;
            _orderService = orderService;
        }

        // Passaggio 1: Mostra il form per l'aggiunta di un nuovo indirizzo o reindirizza alla selezione
        public async Task<IActionResult> AddAddress(bool forceAdd = false)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Se l'utente è autenticato
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var existingAddresses = await _addressService.GetAddressesByUserIdAsync(userId);

                if (existingAddresses.Any() && !forceAdd)
                {
                    // Se l'utente ha già indirizzi, reindirizzalo direttamente alla pagina di selezione
                    return RedirectToAction("SelectAddress");
                }
            }
            else
            {
                // Se l'utente non è autenticato, controlla gli indirizzi basati sul SessionId
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

            // Altrimenti mostra la pagina per aggiungere un nuovo indirizzo
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAddress(Addresses address)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Se l'utente è autenticato, usa l'UserId
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                address.UserId = userId;  // Associa l'indirizzo all'utente autenticato
            }
            else
            {
                // Se l'utente non è autenticato, usa il SessionId
                string sessionId = Request.Cookies["SessionId"];
                if (string.IsNullOrEmpty(sessionId))
                {
                    sessionId = Guid.NewGuid().ToString();
                    Response.Cookies.Append("SessionId", sessionId, new CookieOptions { Expires = DateTime.Now.AddMonths(1) });
                }
                address.SessionId = sessionId;  // Associa l'indirizzo al SessionId dell'utente non autenticato
            }

            await _addressService.AddAddresses(address);

            // Dopo aver salvato l'indirizzo, reindirizza alla selezione dell'indirizzo per l'ordine
            return RedirectToAction("SelectAddress");
        }
        // Passaggio 2: Mostra gli indirizzi salvati per la selezione
        public async Task<IActionResult> SelectAddress()
        {
            IEnumerable<Addresses> addresses;

            if (User.Identity.IsAuthenticated)
            {
                // Se l'utente è autenticato, recupera gli indirizzi basati sull'UserId
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                addresses = await _addressService.GetAddressesByUserIdAsync(userId);
            }
            else
            {
                // Se l'utente non è autenticato, usa il SessionId
                string sessionId = Request.Cookies["SessionId"];
                if (string.IsNullOrEmpty(sessionId))
                {
                    return RedirectToAction("AddAddress");
                }
                addresses = await _addressService.GetAddressesBySessionIdAsync(sessionId);
            }

            // Passa gli indirizzi alla vista
            return View(addresses);
        }

        // Passaggio 2: Creazione dell'ordine prima del pagamento
        [HttpPost]
        public async Task<IActionResult> CreateOrder(int addressId)
        {
            int? userId = null;
            string sessionId = null;
            var cart = default(Cart);  // Variabile per il carrello

            // Controlla se l'utente è autenticato
            if (User.Identity.IsAuthenticated)
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);  // Ottieni l'UserId dell'utente autenticato
                cart = await _cartService.GetCartByUserIdAsync(userId.Value);  // Recupera il carrello per l'utente autenticato
            }
            else
            {
                // Utente non autenticato, usa il SessionId
                sessionId = Request.Cookies["SessionId"];
                if (string.IsNullOrEmpty(sessionId))
                {
                    // Se non c'è il SessionId, creane uno nuovo e memorizzalo nel cookie
                    sessionId = Guid.NewGuid().ToString();
                    Response.Cookies.Append("SessionId", sessionId, new CookieOptions { Expires = DateTime.Now.AddMonths(1) });
                }
                cart = await _cartService.GetCartBySessionIdAsync(sessionId);  // Recupera il carrello per l'utente non loggato
            }

            // Controlla se il carrello esiste e ha elementi
            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest("Il carrello è vuoto.");
            }

            // Calcola il totale dell'ordine
            decimal totalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            // Crea l'ordine
            var order = new Orders
            {
                UserId = userId,  // Se l'utente è autenticato, assegna l'UserId, altrimenti sarà null
                SessionId = sessionId,  // Se l'utente non è autenticato, assegna il SessionId
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending",  // Stato iniziale dell'ordine
                ShippingAddressId = addressId,
                OrderItems = cart.CartItems.Select(ci => new OrderItems
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                }).ToList()
            };

            // Salva l'ordine nel database
            await _orderService.CreateOrderAsync(order);

            // Dopo aver salvato l'ordine, reindirizza alla pagina di pagamento con l'orderId
            return RedirectToAction("Checkout", "Payment", new { orderId = order.OrderId });
        }
    }

    }

