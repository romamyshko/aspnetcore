using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
    public interface IOrdersFilterService
    {
        Task<List<OrderResponse>> GetFilteredOrders(string searchBy, string? searchString);
    }
}
