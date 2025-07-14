namespace OrderManagement.ServiceContracts
{
    public interface IOrdersDeleterService
    {
        Task<bool> DeleteOrderByOrderId(Guid orderId);
    }
}
