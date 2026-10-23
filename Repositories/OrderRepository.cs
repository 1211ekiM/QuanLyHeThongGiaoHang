using DeliveryManagement.Data;
using DeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryManagement.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByCodeAsync(string code);
    Task<Order?> GetDetailsAsync(int id);
    Task<List<Order>> SearchAsync(string? keyword, string? status);
    Task SaveChangesAsync();
}

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _db;

    public OrderRepository(ApplicationDbContext db) => _db = db;

    public Task<Order?> GetByCodeAsync(string code) =>
        _db.Orders.FirstOrDefaultAsync(
            x => x.ExternalOrderCode == code);

    public Task<Order?> GetDetailsAsync(int id) =>
        _db.Orders
            .Include(x => x.Customer)
            .Include(x => x.Details)
                .ThenInclude(x => x.ParcelType)
            .Include(x => x.Shipments)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<Order>> SearchAsync(
        string? keyword, string? status)
    {
        var query = _db.Orders
            .AsNoTracking()
            .Include(x => x.Customer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var k = keyword.Trim();
            query = query.Where(x =>
                x.ExternalOrderCode.Contains(k) ||
                x.Customer.FullName.Contains(k) ||
                x.Customer.Phone.Contains(k));
        }

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(x => x.Status == status);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Take(200)
            .ToListAsync();
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}