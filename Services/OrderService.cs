using DeliveryManagement.Data;
using DeliveryManagement.Models;
using DeliveryManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeliveryManagement.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _db;
    private readonly IOrderRepository _orders;

    public OrderService(
        ApplicationDbContext db,
        IOrderRepository orders)
    {
        _db = db;
        _orders = orders;
    }

    public Task<Order?> FindByCodeAsync(string code)
        => _orders.GetByCodeAsync(code.Trim());

    public async Task ReceiveAsync(int orderId, string userId)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();

        var order = await _db.Orders
            .FirstOrDefaultAsync(x => x.Id == orderId);

        if (order == null)
            throw new InvalidOperationException("Không tìm thấy đơn hàng.");

        if (order.Status != OrderStatuses.NotReceived)
            throw new InvalidOperationException(
                "Chỉ được tiếp nhận đơn chưa nhận.");

        var now = DateTime.UtcNow;

        order.Status = OrderStatuses.Received;
        order.ReceivedAt = now;

        _db.DeliveryStatusHistories.Add(new DeliveryStatusHistory
        {
            OrderId = order.Id,
            OldStatus = OrderStatuses.NotReceived,
            NewStatus = OrderStatuses.Received,
            ChangedByUserId = userId,
            ChangedAt = now,
            Note = "Tiếp nhận hàng hóa"
        });

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }
}