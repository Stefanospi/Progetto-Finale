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

            // Controllo che l'ordine contenga almeno un articolo
            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                throw new ArgumentException("L'ordine deve contenere almeno un articolo.");
            }

            // Controllo che l'importo totale sia positivo
            if (order.TotalAmount <= 0)
            {
                throw new ArgumentException("L'importo totale deve essere positivo.");
            }

            // Verifica dell'indirizzo di spedizione
            if (order.ShippingAddressId <= 0)
            {
                throw new ArgumentException("Indirizzo di spedizione non valido.");
            }

            _dataContext.Orders.Add(order);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Orders> GetOrderByIdAsync(int orderId)
        {
            if (orderId <= 0) throw new ArgumentException("ID ordine non valido.");

            var order = await _dataContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) throw new ArgumentException("Ordine non trovato.");

            return order;
        }

        public async Task UpdateOrderAsync(Orders order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            // Verifica che l'ordine esista
            var existingOrder = await _dataContext.Orders.FindAsync(order.OrderId);
            if (existingOrder == null)
            {
                throw new ArgumentException("L'ordine non esiste.");
            }

            // Aggiorna le proprietà dell'ordine esistente
            existingOrder.Status = order.Status;
            existingOrder.TotalAmount = order.TotalAmount;
            existingOrder.ShippingAddressId = order.ShippingAddressId;
            existingOrder.OrderItems = order.OrderItems;

            _dataContext.Orders.Update(existingOrder);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<List<Orders>> GetOrdersByUserIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("ID utente non valido.");

            var orders = await _dataContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return orders;
        }
    }
}
