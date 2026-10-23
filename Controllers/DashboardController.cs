using DeliveryManagement.Data;
using DeliveryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryManagement.Controllers;

[Authorize(Roles = "Admin,Receptionist")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _db;

    public DashboardController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var today = DateTime.UtcNow.Date;
        var start = today.AddDays(-6);
        var end = today.AddDays(1);

        var vm = new DashboardViewModel
        {
            TotalOrders = await _db.Orders.CountAsync(),

            DeliveringOrders = await _db.Orders.CountAsync(
                x => x.Status == "Delivering"),

            DeliveredOrders = await _db.Orders.CountAsync(
                x => x.Status == "Delivered"),

            // Tổng phí giao đã được ghi nhận thanh toán,
            // không phải tổng giá trị COD.
            CodCollected = await _db.Payments
                .Where(x => x.PaymentPurpose == "CODCollection"
                         && x.PaymentStatus == "Paid")
                .SumAsync(x => (decimal?)x.Amount) ?? 0
        };

        var daily = await _db.Orders
            .Where(x => x.CreatedAt >= start && x.CreatedAt < end)
            .GroupBy(x => x.CreatedAt.Date)
            .Select(g => new
            {
                Date = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        for (var day = start; day < end; day = day.AddDays(1))
        {
            vm.ChartLabels.Add(day.ToString("dd/MM"));
            vm.ChartOrderCounts.Add(
                daily.FirstOrDefault(x => x.Date == day)?.Count ?? 0);
        }

        var statusData = await _db.Orders
            .GroupBy(x => x.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        vm.StatusLabels = statusData.Select(x => x.Status).ToList();
        vm.StatusCounts = statusData.Select(x => x.Count).ToList();

        return View(vm);
    }
}