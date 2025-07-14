using System;
using System.Threading.Tasks;
using ServiceContracts.DTO;

namespace OrderManagement.ServiceContracts
{
    public interface IOrderItemsAdderService
    {
        Task<OrderItemResponse> AddOrderItem(OrderItemAddRequest orderItemRequest);
    }
}
