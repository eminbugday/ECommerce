using ECommerce.Core.DTOs.Orders;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Service;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uow;
    public OrderService(IUnitOfWork uow) => _uow = uow;

    public async Task<OrderDto> CreateAsync(int userId, CreateOrderDto dto)
    {
        var productRepo = _uow.Repository<Product>();
        var order = new Order
        {
            UserId = userId,
            ShippingAddress = dto.ShippingAddress,
            CardHolderName = dto.CardHolderName,
            Status = OrderStatus.Paid // ödeme simülasyonu başarılı kabul edilir
        };

        decimal total = 0m;

        foreach (var item in dto.Items)
        {
            var product = await productRepo.GetByIdAsync(item.ProductId)
                ?? throw new InvalidOperationException($"Ürün bulunamadı (Id: {item.ProductId}).");

            if (product.Stock < item.Quantity)
                throw new InvalidOperationException($"'{product.Name}' için yeterli stok yok.");

            product.Stock -= item.Quantity;
            productRepo.Update(product);

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });

            total += product.Price * item.Quantity;
        }

        order.TotalAmount = total;

        await _uow.Repository<Order>().AddAsync(order);
        await _uow.SaveChangesAsync();

        return (await GetByIdInternalAsync(order.Id))!;
    }

    public async Task<IReadOnlyList<OrderDto>> GetMyOrdersAsync(int userId)
    {
        var list = await OrdersWithItems()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
        return list.Select(ToDto).ToList();
    }

    public async Task<IReadOnlyList<OrderDto>> GetAllAsync()
    {
        var list = await OrdersWithItems()
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
        return list.Select(ToDto).ToList();
    }

    private IQueryable<Order> OrdersWithItems() =>
        _uow.Repository<Order>().Query()
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .AsNoTracking();

    private async Task<OrderDto?> GetByIdInternalAsync(int id)
    {
        var order = await OrdersWithItems().FirstOrDefaultAsync(o => o.Id == id);
        return order is null ? null : ToDto(order);
    }

    private static OrderDto ToDto(Order o) => new()
    {
        Id = o.Id,
        UserId = o.UserId,
        TotalAmount = o.TotalAmount,
        Status = o.Status.ToString(),
        ShippingAddress = o.ShippingAddress,
        CreatedDate = o.CreatedDate,
        Items = o.Items.Select(i => new OrderItemDto
        {
            ProductId = i.ProductId,
            ProductName = i.Product?.Name ?? string.Empty,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList()
    };
}
