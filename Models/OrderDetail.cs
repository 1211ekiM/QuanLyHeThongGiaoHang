using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryManagement.Models;

public class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ParcelTypeId { get; set; }
    public ParcelType ParcelType { get; set; } = null!;

    [StringLength(100)]
    public string? ItemCode { get; set; }

    [StringLength(250)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal WeightKg { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal LengthCm { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal WidthCm { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal HeightCm { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DeclaredValue { get; set; }

    public bool IsFragile { get; set; }
    public bool IsValuable { get; set; }
    public bool IsVehicle { get; set; }
}