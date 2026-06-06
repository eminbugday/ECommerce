using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.DTOs.Orders;

/// <summary>Sepetteki tek bir ürün (ödeme/checkout isteğinde gönderilir).</summary>
public class CartItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

/// <summary>Ödeme formu + sepet -> sipariş oluşturma isteği.</summary>
public class CreateOrderDto
{
    [Required, MinLength(1)]
    public List<CartItemDto> Items { get; set; } = new();

    [Required]
    public string ShippingAddress { get; set; } = string.Empty;

    // Ödeme formu (simülasyon - gerçek kart bilgisi saklanmaz)
    [Required]
    public string CardHolderName { get; set; } = string.Empty;

    [Required, CreditCard]
    public string CardNumber { get; set; } = string.Empty;
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}
