using DeliveryManagement.Models;
using DeliveryManagement.Repositories;
using DeliveryManagement.Services;
using DeliveryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryManagement.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IOrderRepository _orders;
    private readonly IOrderService _service;

    public OrdersController(
        IOrderRepository orders,
        IOrderService service)
    {
        _orders = orders;
        _service = service;
    }

    [HttpGet]
    [Authorize(Policy = "ManageOrders")]
    public async Task<IActionResult> Index(
        string? keyword, string? status)
    {
        var vm = new OrderSearchViewModel
        {
            Keyword = keyword,
            Status = status,
            Orders = await _orders.SearchAsync(keyword, status)
        };

        return View(vm);
    }

    [HttpGet]
    [Authorize(Policy = "ManageOrders")]
    public async Task<IActionResult> Details(int id)
    {
        var order = await _orders.GetDetailsAsync(id);
        return order == null ? NotFound() : View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "ManageOrders")]
    public async Task<IActionResult> Receive(int id)
    {
        try
        {
            await _service.ReceiveAsync(
                id, User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            TempData["Success"] = "Tiếp nhận đơn hàng thành công.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}