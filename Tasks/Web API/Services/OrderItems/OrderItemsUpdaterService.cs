using Microsoft.Extensions.Logging;
using OrderManagement.RepositoryContracts;
using System;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using OrderManagement.ServiceContracts;

namespace OrderManagement.Services
{
    public class OrderItemsUpdaterService : IOrderItemsUpdaterService
    {
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly ILogger<OrderItemsUpdaterService> _logger;

        public OrderItemsUpdaterService(IOrderItemsRepository orderItemsRepository, ILogger<OrderItemsUpdaterService> logger)
        {
            _orderItemsRepository = orderItemsRepository;
            _logger = logger;
        }

        public async Task<OrderItemResponse> UpdateOrderItem(OrderItemUpdateRequest orderItemRequest)
        {
            _logger.LogInformation($"Updating order item. Order Item ID: {orderItemRequest.OrderItemId}...");

            var orderItem = orderItemRequest.ToOrderItem();

            var updatedOrderItem = await _orderItemsRepository.UpdateOrderItem(orderItem);

            _logger.LogInformation($"Order item updated successfully. Order Item ID: {updatedOrderItem.OrderItemId}.");

            return updatedOrderItem.ToOrderItemResponse();
        }
    }
}
