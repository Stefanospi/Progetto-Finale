using Stripe.Checkout;
namespace E_commerce.Services.Interfaces
{
    public interface IStripePaymentService
    {
        Task<Session> CreateCheckoutSessionAsync(int orderId, string currency, decimal totalAmount, string successUrl, string cancelUrl);
    }
}
