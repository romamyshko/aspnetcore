using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
    public interface IOrdersAdderService
    {
        Task<OrderResponse> AddOrder(OrderAddRequest orderRequest);
    }
}
