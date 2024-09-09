using E_commerce.Models.PaymentStripe;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

public class PaymentController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly StripeSetting _stripeSettings;

    public PaymentController(IOptions<StripeSetting> stripeSettings, IOrderService orderService, ICartService cartService)
    {
        _stripeSettings = stripeSettings.Value;
        _orderService = orderService;
        _cartService = cartService;
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(int orderId)
    {
        // Recupera l'ordine dal database
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            return NotFound("Ordine non trovato.");
        }

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Ordine #" + order.OrderId
                    },
                    UnitAmount = (long)(order.TotalAmount * 100), // Prezzo in centesimi
                },
                Quantity = 1,
            },
        },
            Mode = "payment",
            SuccessUrl = Url.Action("Success", "Payment", new { orderId = order.OrderId }, Request.Scheme),
            CancelUrl = Url.Action("Cancel", "Payment", null, Request.Scheme),
        };

        var service = new SessionService();
        Session session = service.Create(options);

        return Json(new { id = session.Id });
    }

    public async Task<IActionResult> Success(int orderId)
    {
        // Recupera l'ordine dal database
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            return NotFound("Ordine non trovato.");
        }

        // Aggiorna lo stato dell'ordine a "Paid"
        order.Status = "Paid";
        await _orderService.UpdateOrderAsync(order);

        // Pulisci il carrello
        await _cartService.ClearCartAsync(order.UserId);

        return View();
    }

    public IActionResult Cancel()
    {
        // Mostra una pagina di pagamento annullato
        return View();
    }
}
