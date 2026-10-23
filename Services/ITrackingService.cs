namespace DeliveryManagement.Services;

public record CheckInRequest(
    int ShipmentId,
    decimal Latitude,
    decimal Longitude,
    decimal? AccuracyMeters,
    string? Note);

public interface ITrackingService
{
    Task SaveLocationAsync(
        CheckInRequest request,
        string userId,
        bool isAdmin);
}