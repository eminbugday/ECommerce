namespace ECommerce.Core.Entities;

public enum OrderStatus
{
    Pending = 0,
    Paid = 1,
    Shipped = 2,
    Cancelled = 3
}

/// <summary>
/// Sepetin onaylanıp ödeme formu doldurulunca oluşan sipariş.
/// </summary>
public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    // Ödeme formundan gelen (örnek/simülasyon) bilgiler
    public string ShippingAddress { get; set; } = string.Empty;
    public string CardHolderName { get; set; } = string.Empty;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
