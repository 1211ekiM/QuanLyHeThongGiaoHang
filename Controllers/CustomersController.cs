using DeliveryManagement.Data;
using DeliveryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryManagement.Controllers;

[Authorize(Roles = "Admin,Receptionist")]
public class CustomersController : Controller
{
    private readonly ApplicationDbContext _db;

    public CustomersController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? keyword)
    {
        var query = _db.Customers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var k = keyword.Trim();
            query = query.Where(x =>
                x.FullName.Contains(k) || x.Phone.Contains(k));
        }

        return View(await query.OrderBy(x => x.FullName)
            .Take(200).ToListAsync());
    }

    [HttpGet]
    public IActionResult Create() => View(new Customer());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Customer customer)
    {
        if (!ModelState.IsValid)
            return View(customer);

        var phoneExists = await _db.Customers
            .AnyAsync(x => x.Phone == customer.Phone);

        if (phoneExists)
        {
            ModelState.AddModelError(
                nameof(customer.Phone), "Số điện thoại đã tồn tại.");
            return View(customer);
        }

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Đã thêm khách hàng.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var customer = await _db.Customers.FindAsync(id);
        return customer == null ? NotFound() : View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();
        if (!ModelState.IsValid) return View(customer);

        var existing = await _db.Customers.FindAsync(id);
        if (existing == null) return NotFound();

        existing.FullName = customer.FullName;
        existing.Phone = customer.Phone;
        existing.Email = customer.Email;
        existing.Address = customer.Address;

        await _db.SaveChangesAsync();

        TempData["Success"] = "Đã cập nhật khách hàng.";
        return RedirectToAction(nameof(Index));
    }
}