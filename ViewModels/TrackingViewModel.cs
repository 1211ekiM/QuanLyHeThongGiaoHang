using DeliveryManagement.Models;

namespace DeliveryManagement.ViewModels;

public class TrackingViewModel
{
    public string OrderCode { get; set; } = string.Empty;
    public string CurrentStatus { get; set; } = string.Empty;

    public List<TrackingLocation> Locations { get; set; } = new();

    public decimal? LatestLatitude =>
        Locations.OrderByDescending(x => x.RecordedAt)
            .FirstOrDefault()?.Latitude;

    public decimal? LatestLongitude =>
        Locations.OrderByDescending(x => x.RecordedAt)
            .FirstOrDefault()?.Longitude;
}