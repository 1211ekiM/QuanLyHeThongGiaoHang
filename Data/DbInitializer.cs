using DeliveryManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeliveryManagement.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var roles = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider
    .GetRequiredService<UserManager<ApplicationUser>>();

        var configuration = scope.ServiceProvider
            .GetRequiredService<IConfiguration>();
        var connection = db.Database.GetDbConnection();

        Console.WriteLine($"SQL Server: {connection.DataSource}");
        Console.WriteLine($"Database: {connection.Database}");

        var pendingMigrations =
            await db.Database.GetPendingMigrationsAsync();

        Console.WriteLine("Pending migrations:");
        foreach (var migration in pendingMigrations)
        {
            Console.WriteLine(migration);
        }

        await db.Database.MigrateAsync();

        string[] roleNames =
        {
            "Admin",
            "Receptionist",
            "DeliveryStaff",
            "Customer"
        };

        foreach (var name in roleNames)
        {
            if (!await roles.RoleExistsAsync(name))
            {
                var result = await roles.CreateAsync(
                    new IdentityRole(name));

                if (!result.Succeeded)
                    throw new InvalidOperationException(
                        $"Không tạo được role {name}.");
            }
        }
        var adminEmail = configuration["AdminSeed:Email"];
        var adminPassword = configuration["AdminSeed:Password"];

        if (!string.IsNullOrWhiteSpace(adminEmail) &&
            !string.IsNullOrWhiteSpace(adminPassword))
        {
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(
                    admin, adminPassword);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join("; ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Không tạo được tài khoản Admin: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    admin, "Admin");

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join("; ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Không gán được role Admin: {errors}");
                }
            }
        }
    }
}