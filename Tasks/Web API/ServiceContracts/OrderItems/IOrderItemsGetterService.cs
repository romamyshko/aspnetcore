using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
    public interface IOrderItemsGetterService
    {
        Task<List<OrderItemResponse>> GetAllOrderItems();

        Task<List<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId);

        Task<OrderItemResponse?> GetOrderItemByOrderItemId(Guid orderItemId);
    }
}
