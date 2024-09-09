using E_commerce.Models.Auth;
using E_commerce.Models.Order;
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

        public CheckoutController(ICartService cartService,IAddressService addressService,IOrderService orderService)
        {
            _cartService = cartService;
            _addressService = addressService;
            _orderService = orderService;
        }

        // Passaggio 1: Mostra il form per l'aggiunta di un nuovo indirizzo o reindirizza alla selezione
        public async Task<IActionResult> AddAddress(bool forceAdd = false)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var existingAddresses = await _addressService.GetAddressesByUserIdAsync(userId);

            if (existingAddresses.Any() && !forceAdd)
            {
                // Se l'utente ha già indirizzi, reindirizzalo direttamente alla pagina di selezione
                return RedirectToAction("SelectAddress");
            }

            // Altrimenti mostra la pagina per aggiungere un nuovo indirizzo
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAddress(Addresses address)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            address.UserId = userId;  // Associa l'indirizzo all'utente autenticato
            await _addressService.AddAddresses(address);

            // Dopo aver salvato l'indirizzo, reindirizza alla selezione dell'indirizzo per l'ordine
            return RedirectToAction("SelectAddress");
        }        // Passaggio 2: Mostra gli indirizzi salvati per la selezione
        public async Task<IActionResult> SelectAddress()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Recupera tutti gli indirizzi associati all'utente
            var addresses = await _addressService.GetAddressesByUserIdAsync(userId);

            // Passa gli indirizzi alla vista
            return View(addresses);
        }

        // Passaggio 2: Creazione dell'ordine prima del pagamento
        [HttpPost]
        public async Task<IActionResult> CreateOrder(int addressId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Recupera il carrello dell'utente
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest("Il carrello è vuoto.");
            }

            // Calcola il totale dell'ordine
            decimal totalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            // Crea l'ordine
            var order = new Orders
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending",  // Imposta lo stato dell'ordine come "Pending"
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

