using System.Security.Claims;
using ECommerce.Core.DTOs.Orders;
using ECommerce.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // sipariş işlemleri için giriş zorunlu
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrdersController(IOrderService orderService) => _orderService = orderService;

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Sepet + ödeme formundan sipariş oluşturur.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        try
        {
            var order = await _orderService.CreateAsync(CurrentUserId, dto);
            return CreatedAtAction(nameof(GetMyOrders), null, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Giriş yapan kullanıcının kendi siparişleri.</summary>
    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders() =>
        Ok(await _orderService.GetMyOrdersAsync(CurrentUserId));

    /// <summary>Admin paneli: tüm siparişler.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _orderService.GetAllAsync());
}
