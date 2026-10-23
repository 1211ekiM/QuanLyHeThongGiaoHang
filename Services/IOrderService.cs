using DeliveryManagement.Models;

namespace DeliveryManagement.Services;

public interface IOrderService
{
    Task<Order?> FindByCodeAsync(string code);
    Task ReceiveAsync(int orderId, string userId);
}