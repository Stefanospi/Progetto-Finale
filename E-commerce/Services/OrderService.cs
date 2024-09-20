using E_commerce.Context;
using E_commerce.Models.Order;
using E_commerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _dataContext;
        public OrderService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task CreateOrderAsync(Orders order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            _dataContext.Orders.Add(order);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<Orders> GetOrderByIdAsync(int orderId)
        {
            return await _dataContext.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.ShippingAddress)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task UpdateOrderAsync(Orders order)
        {
            _dataContext.Orders.Update(order);
            await _dataContext.SaveChangesAsync();
        }
    }
}
