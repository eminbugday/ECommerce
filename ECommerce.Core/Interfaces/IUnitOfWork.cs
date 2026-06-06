using ECommerce.Core.Entities;

namespace ECommerce.Core.Interfaces;

/// <summary>
/// Unit of Work: tek bir transaction altında repository'leri toplar ve değişiklikleri kaydeder.
/// Generic Repository ile birlikte veri erişim katmanının çekirdeğini oluşturur.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : BaseEntity;

    /// <summary>Bekleyen tüm değişiklikleri veritabanına yazar.</summary>
    Task<int> SaveChangesAsync();
}
