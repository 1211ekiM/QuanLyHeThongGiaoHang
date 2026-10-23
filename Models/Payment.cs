using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryManagement.Models;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    [Required, StringLength(30)]
    public string PaymentMethod { get; set; } = "Cash";

    [Required, StringLength(30)]
    public string PaymentPurpose { get; set; } = "ShippingFee";

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required, StringLength(30)]
    public string PaymentStatus { get; set; } = "Pending";

    [StringLength(100)]
    public string? TransactionReference { get; set; }

    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}