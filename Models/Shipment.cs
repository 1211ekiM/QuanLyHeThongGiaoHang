using System.ComponentModel.DataAnnotations;

namespace DeliveryManagement.Models;

public class Shipment
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int DeliveryTypeId { get; set; }
    public DeliveryType DeliveryType { get; set; } = null!;

    public int? DeliveryStaffId { get; set; }
    public DeliveryStaff? DeliveryStaff { get; set; }

    public int? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    [Required, StringLength(50)]
    public string ShipmentCode { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string Status { get; set; } = "Pending";

    public DateTime? ReceivedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }

    [StringLength(500)]
    public string? FailureReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TrackingLocation> TrackingLocations { get; set; }
        = new List<TrackingLocation>();
}