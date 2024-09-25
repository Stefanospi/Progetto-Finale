
using E_commerce.Services.Helper;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class PaymentController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly IStripePaymentService _stripePaymentService;
    private readonly CartHelper _cartHelper;

    public PaymentController(IOrderService orderService, ICartService cartService, IStripePaymentService stripePaymentService,CartHelper cartHelper)
    {
        _orderService = orderService;
        _cartService = cartService;
        _stripePaymentService = stripePaymentService;
        _cartHelper = cartHelper;
    }

    [HttpGet]
    public async Task<IActionResult> Checkout(int orderId)
    {
        try
        {
            // Recupera l'ordine dal database
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Ordine non trovato.");
            }

            // Usa l'importo totale dell'ordine per il pagamento
            var totalAmount = order.TotalAmount;

            // Creazione della sessione di pagamento Stripe
            var session = await _stripePaymentService.CreateCheckoutSessionAsync(
                orderId,
                "eur", // Moneta
                totalAmount,  // Usa il prezzo reale dell'ordine
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
            var cart = await _cartService.GetCartByUserIdAsync(order.UserId.Value);
            if (cart != null)
            {
                await _cartService.ClearCartAsync(cart.CartId);  // Svuota il carrello autenticato
            }
        }
        else
        {
            string sessionId = Request.Cookies["SessionId"];
            if (!string.IsNullOrEmpty(sessionId))
            {
                await _cartService.ClearCartNoLogginAsync(sessionId);  // Svuota il carrello non autenticato
            }
        }
        // Passa l'ordine alla vista per mostrarlo
        return View(order);
    }
    public async Task<IActionResult> Cancel()
    {
        // Usa CartHelper per aggiornare il conteggio degli articoli nel carrello dopo la rimozione
        ViewBag.CartItemCount = await _cartHelper.GetCartItemCountAsync(User);
        // Mostra una pagina di pagamento annullato
        return View();
    }
}
