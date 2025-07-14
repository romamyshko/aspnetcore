using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
    public interface IOrdersUpdaterService
    {
        Task<OrderResponse> UpdateOrder(OrderUpdateRequest orderRequest);
    }
}
