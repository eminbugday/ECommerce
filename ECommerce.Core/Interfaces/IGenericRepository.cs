using System.Linq.Expressions;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Interfaces;

/// <summary>
/// Generic Repository Pattern. Her entity için ortak veri erişim operasyonları.
/// SOLID -> Interface Segregation + Dependency Inversion: servisler bu soyutlamaya bağımlıdır,
/// somut EF Core sınıfına değil.
/// </summary>
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();

    /// <summary>Koşula göre filtreli sorgu (ör. ürün filtreleme).</summary>
    Task<IReadOnlyList<T>> WhereAsync(Expression<Func<T, bool>> predicate);

    /// <summary>İlişkili tabloları da getirmek için sorgu kaynağı (Include kullanımı için).</summary>
    IQueryable<T> Query();

    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
