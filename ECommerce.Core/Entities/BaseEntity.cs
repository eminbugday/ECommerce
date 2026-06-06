namespace ECommerce.Core.Entities;

/// <summary>
/// Tüm entity'lerin ortak alanları. Generic Repository bu tip üzerinden çalışır.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
}
