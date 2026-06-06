using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Service;

/// <summary>
/// Tüm uygulama servislerini tek satırda kaydeder (Program.cs sade kalsın).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Veri erişim
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // İş servisleri
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
