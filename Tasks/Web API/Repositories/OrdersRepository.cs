using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Entities;
using OrderManagement.RepositoryContracts;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderManagement.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<OrdersRepository> _logger;

        public OrdersRepository(ApplicationDbContext db, ILogger<OrdersRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Order> AddOrder(Order order)
        {
            _logger.LogInformation("Adding order to the database...");

            _db.Orders.Add(order);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Order added successfully.");

            return order;
        }

        public async Task<bool> DeleteOrderByOrderId(Guid orderId)
        {
            _logger.LogInformation($"Deleting order with ID: {orderId}...");

            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning($"Order not found with ID: {orderId}.");
                return false;
            }

            _db.Orders.Remove(order);

            await _db.SaveChangesAsync();

            _logger.LogInformation($"Order deleted successfully. ID: {orderId}.");

            return true;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            _logger.LogInformation("Retrieving all orders...");

            var orders = await _db.Orders.OrderByDescending(temp => temp.OrderDate).ToListAsync();

            _logger.LogInformation($"Retrieved {orders.Count} orders successfully.");

            return orders;
        }

        public async Task<List<Order>> GetFilteredOrders(Expression<Func<Order, bool>> predicate)
        {
            _logger.LogInformation("Retrieving filtered orders...");

            var filteredOrders = await _db.Orders.Where(predicate)
                .OrderByDescending(temp => temp.OrderDate).ToListAsync();

            _logger.LogInformation($"Retrieved {filteredOrders.Count} filtered orders successfully.");

            return filteredOrders;
        }

        public async Task<Order?> GetOrderByOrderId(Guid orderId)
        {
            _logger.LogInformation($"Retrieving order with ID: {orderId}...");

            var order = await _db.Orders.FindAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning($"Order not found with ID: {orderId}.");
            }
            else
            {
                _logger.LogInformation($"Order retrieved successfully. ID: {orderId}.");
            }

            return order;
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            _logger.LogInformation($"Updating order with ID: {order.OrderId}...");

            var existingOrder = await _db.Orders.FindAsync(order.OrderId);
            if (existingOrder == null)
            {
                _logger.LogWarning($"Order not found with ID: {order.OrderId}.");
                return order;
            }

            existingOrder.OrderNumber = order.OrderNumber;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.CustomerName = order.CustomerName;
            existingOrder.TotalAmount = order.TotalAmount;

            await _db.SaveChangesAsync();

            _logger.LogInformation($"Order updated successfully. ID: {order.OrderId}.");

            return order;
        }
    }
}
