using ECommerce.Core.Entities;
using ECommerce.Repository;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Service;

/// <summary>
/// Uygulama açılışında varsayılan admin kullanıcısını ve örnek ürünleri oluşturur.
/// (Şifre hash'i BCrypt ile üretildiği için HasData yerine burada seed ediyoruz.)
/// </summary>
public static class DbInitializer
{
    public const string AdminEmail = "admin@admin.com";
    public const string AdminPassword = "Admin123!";

    public static async Task SeedAsync(AppDbContext context)
    {
        if (!await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
        {
            context.Users.Add(new User
            {
                FullName = "Sistem Yöneticisi",
                Email = AdminEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(AdminPassword),
                Role = UserRole.Admin,
                IsActive = true
            });
            await context.SaveChangesAsync();
        }

        if (!await context.Products.AnyAsync())
        {
            context.Products.AddRange(
                new Product { Name = "Kablosuz Kulaklık", Description = "Bluetooth 5.0", Price = 749.90m, Stock = 50, CategoryId = 1, ImageUrl = "" },
                new Product { Name = "Pamuklu Tişört", Description = "%100 pamuk", Price = 199.90m, Stock = 120, CategoryId = 2, ImageUrl = "" },
                new Product { Name = "Temiz Kod", Description = "Yazılım kitabı", Price = 149.90m, Stock = 30, CategoryId = 3, ImageUrl = "" }
            );
            await context.SaveChangesAsync();
        }
    }
}
