using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Entities;
using OrderManagement.RepositoryContracts;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderManagement.Repositories
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<OrderItemsRepository> _logger;

        public OrderItemsRepository(ApplicationDbContext db, ILogger<OrderItemsRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<OrderItem> AddOrderItem(OrderItem orderItem)
        {
            _logger.LogInformation("Adding order item to the database...");

            _db.OrderItems.Add(orderItem);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Order item with ID {OrderItemId} added to the database.", orderItem.OrderItemId);

            return orderItem;
        }

        public async Task<bool> DeleteOrderItemByOrderItemId(Guid orderItemId)
        {
            _logger.LogInformation("Deleting order item from the database...");

            var orderItem = await _db.OrderItems.FindAsync(orderItemId);
            if (orderItem == null)
            {
                _logger.LogWarning($"Order item not found with ID: {orderItemId}.");
                return false;
            }

            _db.OrderItems.Remove(orderItem);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Order item with ID {OrderItemId} deleted from the database.", orderItemId);

            return true;
        }

        public async Task<List<OrderItem>> GetAllOrderItems()
        {
            _logger.LogInformation("Retrieving all order items...");

            var orderItems = await _db.OrderItems.OrderBy(temp => temp.OrderId).ToListAsync();

            _logger.LogInformation($"Retrieved {orderItems.Count} order items successfully.");

            return orderItems;
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(Guid orderId)
        {
            _logger.LogInformation("Retrieving order items by OrderId...");

            var orderItems = await _db.OrderItems.Where(oi => oi.OrderId == orderId).ToListAsync();

            _logger.LogInformation($"Retrieved {orderItems.Count} order items associated with OrderId: {orderId}.");

            return orderItems;
        }

        public async Task<OrderItem?> GetOrderItemByOrderItemId(Guid orderItemId)
        {
            _logger.LogInformation("Retrieving order item by OrderItemId...");

            var orderItem = await _db.OrderItems.FindAsync(orderItemId);

            if (orderItem == null)
            {
                _logger.LogWarning($"Order item not found with ID: {orderItemId}.");
            }
            else
            {
                _logger.LogInformation("Order item retrieved successfully.");
            }

            return orderItem;
        }

        public async Task<OrderItem> UpdateOrderItem(OrderItem orderItem)
        {
            _logger.LogInformation("Updating order item in the database...");

            var existingOrderItem = await _db.OrderItems.FindAsync(orderItem.OrderItemId);
            if (existingOrderItem == null)
            {
                throw new ArgumentException($"Order item with ID {orderItem.OrderItemId} does not exist.");
            }

            existingOrderItem.OrderId = orderItem.OrderId;
            existingOrderItem.ProductName = orderItem.ProductName;
            existingOrderItem.Quantity = orderItem.Quantity;
            existingOrderItem.UnitPrice = orderItem.UnitPrice;
            existingOrderItem.TotalPrice = orderItem.TotalPrice;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Order item with ID {OrderItemId} updated in the database.", orderItem.OrderItemId);

            return existingOrderItem;
        }
    }
}
