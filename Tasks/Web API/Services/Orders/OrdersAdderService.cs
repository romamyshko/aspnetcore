using Microsoft.Extensions.Logging;
using OrderManagement.RepositoryContracts;
using OrderManagement.ServiceContracts;
using ServiceContracts.DTO;

namespace OrderManagement.Services
{
    public class OrdersAdderService : IOrdersAdderService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly ILogger<OrdersAdderService> _logger;

        public OrdersAdderService(IOrdersRepository ordersRepository, IOrderItemsRepository orderItemsRepository, ILogger<OrdersAdderService> logger)
        {
            _ordersRepository = ordersRepository;
            _orderItemsRepository = orderItemsRepository;
            _logger = logger;
        }

        public async Task<OrderResponse> AddOrder(OrderAddRequest orderRequest)
        {
            _logger.LogInformation("Adding a new order");

            var order = orderRequest.ToOrder();
            order.OrderId = Guid.NewGuid();

            var addedOrder = await _ordersRepository.AddOrder(order);
            var addedOrderResponse = addedOrder.ToOrderResponse();

            foreach (var item in orderRequest.OrderItems)
            {
                var orderItem = item.ToOrderItem();
                orderItem.OrderItemId = Guid.NewGuid();
                orderItem.OrderId = addedOrder.OrderId;

                var addedOrderItem = await _orderItemsRepository.AddOrderItem(orderItem);
                addedOrderResponse.OrderItems.Add(addedOrderItem.ToOrderItemResponse());
            }

            _logger.LogInformation("Order added successfully");

            return addedOrderResponse;
        }
    }
}
