using E_commerce.Services.Interfaces;
using Stripe;
using Stripe.Checkout;
namespace E_commerce.Services
{
    public class StripePaymentService : IStripePaymentService
    {
        private readonly IOrderService _orderService;

        public StripePaymentService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Session> CreateCheckoutSessionAsync(int orderId, string currency, decimal totalAmount, string successUrl, string cancelUrl)
        {
            // Recupera l'ordine dal database
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new ArgumentException("Ordine non trovato.");
            }

            // Creazione delle opzioni della sessione di pagamento
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Ordine #" + order.OrderId
                        },
                        UnitAmount = (long)(totalAmount * 100), // Prezzo in centesimi
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }
    }
}
