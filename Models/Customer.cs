using System.ComponentModel.DataAnnotations;

namespace DeliveryManagement.Models;

public class Customer
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress, StringLength(256)]
    public string? Email { get; set; }

    [Required, StringLength(500)]
    public string Address { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}