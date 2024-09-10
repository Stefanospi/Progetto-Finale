
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

public class PaymentController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly IStripePaymentService _stripePaymentService;

    public PaymentController(IOrderService orderService, ICartService cartService, IStripePaymentService stripePaymentService)
    {
        _orderService = orderService;
        _cartService = cartService;
        _stripePaymentService = stripePaymentService;
    }

    [HttpGet]
    public async Task<IActionResult> Checkout(int orderId)
    {
        try
        {
            // Creazione della sessione di pagamento
            var session = await _stripePaymentService.CreateCheckoutSessionAsync(
                orderId,
                "usd", // Moneta
                100m,  // Sostituisci con il prezzo reale del prodotto o dell'ordine
                Url.Action("Success", "Payment", new { orderId = orderId }, Request.Scheme),
                Url.Action("Cancel", "Payment", null, Request.Scheme)
            );

            // Reindirizza l'utente all'URL di pagamento Stripe
            return Redirect(session.Url);
        }
        catch (Exception ex)
        {
            // Gestisci errori
            return BadRequest(ex.Message);
        }
    }
    [HttpGet]
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

        // Controlla se l'utente è autenticato
        if (User.Identity.IsAuthenticated)
        {
            // Svuota il carrello usando l'UserId
            await _cartService.ClearCartAsync(order.UserId);
        }
        else
        {
            //// Usa il SessionId per identificare il carrello degli utenti non loggati
            //string sessionId = Request.Cookies["SessionId"];
            //if (!string.IsNullOrEmpty(sessionId))
            //{
            //    await _cartService.ClearCartAsync(sessionId);
            //}
        }

        // Passa l'ordine alla vista per mostrarlo
        return View(order);
    }
    public IActionResult Cancel()
    {
        // Mostra una pagina di pagamento annullato
        return View();
    }
}
