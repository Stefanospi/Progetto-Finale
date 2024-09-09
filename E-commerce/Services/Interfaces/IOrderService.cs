using E_commerce.Models.Order;

namespace E_commerce.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(Orders orders);
        Task<Orders> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Orders order);
    }
}
