using ECommerce.Core.DTOs.Orders;

namespace ECommerce.Core.Services;

public interface IOrderService
{
    /// <summary>Sepet + ödeme formundan sipariş oluşturur, stok düşer.</summary>
    Task<OrderDto> CreateAsync(int userId, CreateOrderDto dto);

    /// <summary>Giriş yapan kullanıcının kendi siparişleri.</summary>
    Task<IReadOnlyList<OrderDto>> GetMyOrdersAsync(int userId);

    /// <summary>Admin paneli: tüm siparişler.</summary>
    Task<IReadOnlyList<OrderDto>> GetAllAsync();
}
