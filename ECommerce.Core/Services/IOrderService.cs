using ECommerce.Core.DTOs.Orders;

namespace ECommerce.Core.Services;

public interface IOrderService
{
    Task<OrderDto> CreateAsync(int userId, CreateOrderDto dto);

    Task<IReadOnlyList<OrderDto>> GetMyOrdersAsync(int userId);

    Task<IReadOnlyList<OrderDto>> GetAllAsync();
}
