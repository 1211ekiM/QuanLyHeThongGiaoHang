namespace DeliveryManagement.ViewModels;

public class DashboardViewModel
{
    public int TotalOrders { get; set; }
    public int DeliveringOrders { get; set; }
    public int DeliveredOrders { get; set; }
    public decimal CodCollected { get; set; }

    public List<string> ChartLabels { get; set; } = new();
    public List<int> ChartOrderCounts { get; set; } = new();

    public List<string> StatusLabels { get; set; } = new();
    public List<int> StatusCounts { get; set; } = new();
}