namespace ECommerce.Core.Entities;

/// <summary>
/// Ürün kategorisi. Ürün filtreleme bu alan üzerinden yapılır.
/// </summary>
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
