using Microsoft.Extensions.Logging;
using OrderManagement.Entities;
using OrderManagement.RepositoryContracts;
using OrderManagement.ServiceContracts;
using ServiceContracts.DTO;

namespace OrderManagement.Services
{
    public class OrdersFilterService : IOrdersFilterService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger<OrdersFilterService> _logger;

        public OrdersFilterService(IOrdersRepository ordersRepository, ILogger<OrdersFilterService> logger)
        {
            _ordersRepository = ordersRepository;
            _logger = logger;
        }

        public async Task<List<OrderResponse>> GetFilteredOrders(string searchBy, string? searchString)
        {
            _logger.LogInformation($"Filtering orders by {searchBy}: {searchString}");

            List<Order> filteredOrders;
            switch (searchBy)
            {
                case nameof(OrderResponse.CustomerName):

                    filteredOrders = await _ordersRepository.GetFilteredOrders(o => o.CustomerName.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                    break;
                case nameof(OrderResponse.OrderDate):

                    filteredOrders = await _ordersRepository.GetFilteredOrders(o => o.OrderDate.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase));
                    break;
                case nameof(OrderResponse.OrderNumber):

                    filteredOrders = await _ordersRepository.GetFilteredOrders(o => o.OrderNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                    break;
                default:

                    _logger.LogWarning($"Invalid search field: {searchBy}");
                    return new List<OrderResponse>();
            }

            var orderResponses = filteredOrders.ToOrderResponseList();

            _logger.LogInformation($"Filtered orders by {searchBy}: {searchString} retrieved successfully");

            return orderResponses;
        }
    }
}
