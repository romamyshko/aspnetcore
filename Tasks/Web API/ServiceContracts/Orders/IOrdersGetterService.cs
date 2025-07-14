using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
    public interface IOrdersGetterService
    {
        Task<List<OrderResponse>> GetAllOrders();

        Task<OrderResponse?> GetOrderByOrderId(Guid orderId);
    }
}
