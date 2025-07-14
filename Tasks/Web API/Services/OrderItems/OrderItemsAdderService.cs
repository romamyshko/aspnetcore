using Microsoft.Extensions.Logging;
using OrderManagement.RepositoryContracts;
using System;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using OrderManagement.ServiceContracts;

namespace OrderManagement.Services
{
    public class OrderItemsAdderService : IOrderItemsAdderService
    {
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly ILogger<OrderItemsAdderService> _logger;

        public OrderItemsAdderService(IOrderItemsRepository orderItemsRepository, ILogger<OrderItemsAdderService> logger)
        {
            _orderItemsRepository = orderItemsRepository;
            _logger = logger;
        }

        public async Task<OrderItemResponse> AddOrderItem(OrderItemAddRequest orderItemRequest)
        {
            _logger.LogInformation("Adding order item...");

            var orderItem = orderItemRequest.ToOrderItem();

            orderItem.OrderItemId = Guid.NewGuid();

            var addedOrderItem = await _orderItemsRepository.AddOrderItem(orderItem);

            _logger.LogInformation($"Order item added successfully. Order Item ID: {addedOrderItem.OrderItemId}.");

            return addedOrderItem.ToOrderItemResponse();
        }
    }
}
