using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryManagement.Models;

public static class OrderStatuses
{
    public const string NotReceived = "NotReceived";
    public const string Received = "Received";
    public const string Delivering = "Delivering";
    public const string Delivered = "Delivered";
    public const string DeliveryFailed = "DeliveryFailed";
    public const string Returned = "Returned";
}

public class Order
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string ExternalOrderCode { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    [Required, StringLength(30)]
    public string Status { get; set; } = OrderStatuses.NotReceived;

    [Column(TypeName = "decimal(10,2)")]
    public decimal DistanceKm { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingFee { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CodAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReceivedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }

    public ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();
    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<DeliveryStatusHistory> StatusHistories { get; set; }
        = new List<DeliveryStatusHistory>();
}