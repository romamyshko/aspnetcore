using System;
using System.Threading.Tasks;
using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
    public interface IOrderItemsUpdaterService
    {
        Task<OrderItemResponse> UpdateOrderItem(OrderItemUpdateRequest orderItemRequest);
    }
}
