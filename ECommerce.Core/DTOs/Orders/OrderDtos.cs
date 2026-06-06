using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.DTOs.Orders;

public class CartItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

public class CreateOrderDto
{
    [Required, MinLength(1)]
    public List<CartItemDto> Items { get; set; } = new();

    [Required]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required]
    public string CardHolderName { get; set; } = string.Empty;

    [Required, StringLength(16, MinimumLength = 16, ErrorMessage = "Kart numarası 16 hane olmalıdır.")]
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
    public string UserFullName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class ApproveOrderDto
{
    public bool Approved { get; set; }
    public string? RejectionReason { get; set; }
}
