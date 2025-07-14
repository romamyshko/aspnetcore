using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using OrderManagement.ServiceContracts;
using OrderManagement.RepositoryContracts;

namespace OrderManagement.Services
{
    public class OrderItemsGetterService : IOrderItemsGetterService
    {
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly ILogger<OrderItemsGetterService> _logger;

        public OrderItemsGetterService(IOrderItemsRepository orderItemsRepository, ILogger<OrderItemsGetterService> logger)
        {
            _orderItemsRepository = orderItemsRepository;
            _logger = logger;
        }

        public async Task<List<OrderItemResponse>> GetAllOrderItems()
        {
            _logger.LogInformation("Retrieving all order items...");

            var orderItems = await _orderItemsRepository.GetAllOrderItems();

            _logger.LogInformation("All order items retrieved successfully.");

            return orderItems.ToOrderItemResponseList();
        }

        public async Task<List<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId)
        {
            _logger.LogInformation($"Retrieving order items for Order ID: {orderId}...");

            var orderItems = await _orderItemsRepository.GetOrderItemsByOrderId(orderId);

            _logger.LogInformation($"Order items retrieved successfully for Order ID: {orderId}.");

            return orderItems.ToOrderItemResponseList();
        }

        public async Task<OrderItemResponse?> GetOrderItemByOrderItemId(Guid orderItemId)
        {
            _logger.LogInformation($"Retrieving order item by Order Item ID: {orderItemId}...");

            var orderItem = await _orderItemsRepository.GetOrderItemByOrderItemId(orderItemId);

            if (orderItem == null)
            {
                _logger.LogWarning($"Order item not found for Order Item ID: {orderItemId}.");
            }
            else
            {
                _logger.LogInformation($"Order item retrieved successfully. Order Item ID: {orderItemId}.");
            }

            return orderItem?.ToOrderItemResponse();
        }
    }
}
