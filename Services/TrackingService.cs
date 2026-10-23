using DeliveryManagement.Data;
using DeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryManagement.Services;

public class TrackingService : ITrackingService
{
    private readonly ApplicationDbContext _db;

    public TrackingService(ApplicationDbContext db) => _db = db;

    public async Task SaveLocationAsync(
        CheckInRequest request,
        string userId,
        bool isAdmin)
    {
        if (request.Latitude < -90 || request.Latitude > 90)
            throw new ArgumentException("Latitude không hợp lệ.");

        if (request.Longitude < -180 || request.Longitude > 180)
            throw new ArgumentException("Longitude không hợp lệ.");

        if (request.AccuracyMeters < 0)
            throw new ArgumentException("Độ chính xác không hợp lệ.");

        var shipment = await _db.Shipments
            .Include(x => x.DeliveryStaff)
            .FirstOrDefaultAsync(x => x.Id == request.ShipmentId);

        if (shipment == null)
            throw new InvalidOperationException("Không tìm thấy chuyến giao.");

        if (!isAdmin &&
            shipment.DeliveryStaff?.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                "Bạn không được phân công chuyến giao này.");
        }

        if (shipment.Status != "Delivering")
            throw new InvalidOperationException(
                "Chỉ cho phép check-in khi đang giao hàng.");

        _db.TrackingLocations.Add(new TrackingLocation
        {
            ShipmentId = shipment.Id,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            AccuracyMeters = request.AccuracyMeters,
            RecordedAt = DateTime.UtcNow,
            Note = request.Note
        });

        await _db.SaveChangesAsync();
    }
}