using ECommerce.Core.Entities;
using ECommerce.Repository;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Service;

public static class DbInitializer
{
    public const string AdminEmail = "admin@admin.com";
    public const string AdminPassword = "Admin123!";

    public static async Task SeedAsync(AppDbContext context)
    {
        // Admin user
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

        // "Otomotiv" category — seeded at runtime so no migration needed
        var autoCat = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Otomotiv");
        if (autoCat == null)
        {
            autoCat = new Category { Name = "Otomotiv", CreatedDate = DateTime.UtcNow };
            context.Categories.Add(autoCat);
            await context.SaveChangesAsync();
        }

        // Car products (only if no car products exist yet)
        if (!await context.Products.AnyAsync(p => p.CategoryId == autoCat.Id))
        {
            context.Products.AddRange(
                new Product
                {
                    Name = "BMW M3 Competition",
                    Description = "503 HP · Sıralı 6 Silindir · 0–100 km/s 3.9 sn · Otomatik vites",
                    Price = 4_250_000m,
                    Stock = 2,
                    CategoryId = autoCat.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1555215695-3004980ad54e?w=640&q=80",
                    CreatedDate = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Mercedes-AMG C63 S",
                    Description = "510 HP · 4MATIC+ AWD · Sport paket dahil · Gri iç döşeme",
                    Price = 3_890_000m,
                    Stock = 3,
                    CategoryId = autoCat.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1618843479313-40f8afb4b4d8?w=640&q=80",
                    CreatedDate = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Porsche 911 Carrera S",
                    Description = "450 HP · Boxer 6 · PDK şanzıman · Spor Chrono paketi",
                    Price = 6_100_000m,
                    Stock = 1,
                    CategoryId = autoCat.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1503376780353-7e6692767b70?w=640&q=80",
                    CreatedDate = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Ferrari 488 GTB",
                    Description = "670 HP · V8 Turbo · 330 km/s max hız · Carbon paketi",
                    Price = 9_500_000m,
                    Stock = 1,
                    CategoryId = autoCat.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1592198084033-aade902d1aae?w=640&q=80",
                    CreatedDate = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Lamborghini Huracán EVO",
                    Description = "640 HP · V10 Doğal emişli · AWD · Verde Mantis renk",
                    Price = 11_200_000m,
                    Stock = 1,
                    CategoryId = autoCat.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1544636331-e26879cd4d9b?w=640&q=80",
                    CreatedDate = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Audi RS6 Avant",
                    Description = "630 HP · Quattro AWD · Station gövde · Siyah optik",
                    Price = 3_400_000m,
                    Stock = 4,
                    CategoryId = autoCat.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1603584173870-7f23fdae1b7a?w=640&q=80",
                    CreatedDate = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
