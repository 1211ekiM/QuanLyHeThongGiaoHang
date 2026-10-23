using DeliveryManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliveryManagement.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<DeliveryType> DeliveryTypes => Set<DeliveryType>();
    public DbSet<DeliveryStaff> DeliveryStaff => Set<DeliveryStaff>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<ParcelType> ParcelTypes => Set<ParcelType>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<TrackingLocation> TrackingLocations => Set<TrackingLocation>();
    public DbSet<DeliveryStatusHistory> DeliveryStatusHistories
        => Set<DeliveryStatusHistory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Order>()
            .HasIndex(x => x.ExternalOrderCode)
            .IsUnique();

        builder.Entity<DeliveryStaff>()
            .HasIndex(x => x.UserId)
            .IsUnique();

        builder.Entity<DeliveryStaff>()
            .HasOne(x => x.User)
            .WithOne(x => x.DeliveryStaff)
            .HasForeignKey<DeliveryStaff>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DeliveryStaff>()
            .HasIndex(x => x.StaffCode)
            .IsUnique();

        builder.Entity<Vehicle>()
            .HasIndex(x => x.PlateNumber)
            .IsUnique();

        builder.Entity<ParcelType>()
            .HasIndex(x => x.Name)
            .IsUnique();

        builder.Entity<DeliveryType>()
            .HasIndex(x => x.Name)
            .IsUnique();

        builder.Entity<Order>()
            .Property(x => x.ShippingFee)
            .HasPrecision(18, 2);

        builder.Entity<Order>()
            .Property(x => x.CodAmount)
            .HasPrecision(18, 2);

        builder.Entity<Order>()
            .Property(x => x.DistanceKm)
            .HasPrecision(10, 2);

        builder.Entity<TrackingLocation>()
            .Property(x => x.Latitude)
            .HasPrecision(10, 7);

        builder.Entity<TrackingLocation>()
            .Property(x => x.Longitude)
            .HasPrecision(10, 7);

        builder.Entity<Order>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Orders_Status",
                "[Status] IN ('NotReceived','Received','Delivering'," +
                "'Delivered','DeliveryFailed','Returned')"));

        builder.Entity<TrackingLocation>()
            .ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Tracking_Latitude",
                    "[Latitude] BETWEEN -90 AND 90");
                t.HasCheckConstraint(
                    "CK_Tracking_Longitude",
                    "[Longitude] BETWEEN -180 AND 180");
            });
    }
}