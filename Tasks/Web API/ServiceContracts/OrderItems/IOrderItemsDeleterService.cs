using System;
using System.Threading.Tasks;

namespace OrderManagement.ServiceContracts
{
    public interface IOrderItemsDeleterService
    {
        Task<bool> DeleteOrderItemByOrderItemId(Guid orderItemId);
    }
}
