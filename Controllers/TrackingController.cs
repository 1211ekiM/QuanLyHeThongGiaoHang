using System.Security.Claims;
using DeliveryManagement.Data;
using DeliveryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryManagement.Controllers;

[Authorize(Roles = "Admin,DeliveryStaff")]
public class TrackingController : Controller
{
    private readonly ITrackingService _tracking;
    private readonly ApplicationDbContext _db;

    public TrackingController(
        ITrackingService tracking,
        ApplicationDbContext db)
    {
        _tracking = tracking;
        _db = db;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckIn(
        CheckInRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (userId == null) return Challenge();

            await _tracking.SaveLocationAsync(
                request,
                userId,
                User.IsInRole("Admin"));

            return Json(new { success = true });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> History(int shipmentId)
    {
        var query = _db.Shipments
            .Where(x => x.Id == shipmentId);

        if (!User.IsInRole("Admin"))
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            query = query.Where(x =>
                x.DeliveryStaff != null &&
                x.DeliveryStaff.UserId == userId);
        }

        var shipment = await query
            .Select(x => new
            {
                x.Id,
                x.ShipmentCode,
                Locations = x.TrackingLocations
                    .OrderByDescending(l => l.RecordedAt)
                    .Take(100)
                    .Select(l => new
                    {
                        l.Latitude,
                        l.Longitude,
                        l.RecordedAt,
                        l.AccuracyMeters,
                        l.Note
                    }).ToList()
            })
            .FirstOrDefaultAsync();

        return shipment == null ? NotFound() : Json(shipment);
    }
}