using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAuthService _authService;
        public OrderController(IOrderService orderService,IAuthService authService)
        {
            _orderService = orderService;
            _authService = authService;
        }
        // Aggiungi un attributo di routing esplicito per evitare conflitti
        [HttpGet("order/details/{orderId}")]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound("Ordine non trovato.");
            }

            return View(order); // Passa l'ordine alla vista dei dettagli
        }
    }
}
