using System.Linq.Expressions;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Interfaces;
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();

    Task<IReadOnlyList<T>> WhereAsync(Expression<Func<T, bool>> predicate);

    IQueryable<T> Query();

    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
