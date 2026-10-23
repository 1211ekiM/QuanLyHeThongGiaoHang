using DeliveryManagement.Models;

namespace DeliveryManagement.ViewModels;

public class OrderSearchViewModel
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public List<Order> Orders { get; set; } = new();
}