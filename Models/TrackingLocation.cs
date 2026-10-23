using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryManagement.Models;

public class TrackingLocation
{
    public long Id { get; set; }

    public int ShipmentId { get; set; }
    public Shipment Shipment { get; set; } = null!;

    [Column(TypeName = "decimal(10,7)")]
    public decimal Latitude { get; set; }

    [Column(TypeName = "decimal(10,7)")]
    public decimal Longitude { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? AccuracyMeters { get; set; }

    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    [StringLength(250)]
    public string? Note { get; set; }
}