namespace ECommerce.Core.Entities;

/// <summary>
/// Sipariş içindeki tek bir ürün satırı (sepet kalemi).
/// </summary>
public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
