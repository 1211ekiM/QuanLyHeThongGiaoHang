using System.ComponentModel.DataAnnotations;

namespace DeliveryManagement.Models;

public class DeliveryStaff
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    [Required, StringLength(30)]
    public string StaffCode { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}