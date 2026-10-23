using System.ComponentModel.DataAnnotations;

namespace DeliveryManagement.Models;

public class DeliveryStatusHistory
{
    public long Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    [StringLength(30)]
    public string? OldStatus { get; set; }

    [Required, StringLength(30)]
    public string NewStatus { get; set; } = string.Empty;

    public string? ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    [StringLength(500)]
    public string? Note { get; set; }
}