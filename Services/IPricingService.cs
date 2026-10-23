using DeliveryManagement.Models;

namespace DeliveryManagement.Services;

public interface IPricingService
{
    decimal CalculateFee(
        Order order,
        IReadOnlyCollection<OrderDetail> details,
        decimal pricePerKg,
        decimal pricePerKm,
        decimal oversizeSurcharge,
        decimal fragileSurcharge,
        decimal valuableSurcharge,
        decimal vehicleSurcharge,
        decimal expressSurcharge);
}

public class PricingService : IPricingService
{
    public decimal CalculateFee(
        Order order,
        IReadOnlyCollection<OrderDetail> details,
        decimal pricePerKg,
        decimal pricePerKm,
        decimal oversizeSurcharge,
        decimal fragileSurcharge,
        decimal valuableSurcharge,
        decimal vehicleSurcharge,
        decimal expressSurcharge)
    {
        decimal total = 0;

        foreach (var item in details)
        {
            total += item.ParcelType.BaseFee;
            total += item.WeightKg * pricePerKg;

            var volume = item.LengthCm
                       * item.WidthCm
                       * item.HeightCm;

            if (volume > 100000)
                total += oversizeSurcharge;

            if (item.IsFragile)
                total += fragileSurcharge;

            if (item.IsValuable)
                total += valuableSurcharge;

            if (item.IsVehicle)
                total += vehicleSurcharge;
        }

        total += order.DistanceKm * pricePerKm;
        total += expressSurcharge;

        return decimal.Round(total, 0, MidpointRounding.AwayFromZero);
    }
}