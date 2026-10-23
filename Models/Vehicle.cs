using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryManagement.Models;

public class Vehicle
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public string PlateNumber { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string VehicleType { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal MaxLoadKg { get; set; }

    public bool IsActive { get; set; } = true;
}