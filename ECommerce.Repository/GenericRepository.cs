using System.Linq.Expressions;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repository;

/// <summary>
/// IGenericRepository'nin EF Core implementasyonu. Tek bir sınıf tüm entity'lere hizmet eder.
/// </summary>
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task<IReadOnlyList<T>> GetAllAsync() =>
        await _dbSet.AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<T>> WhereAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

    public IQueryable<T> Query() => _dbSet.AsQueryable();

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public void Update(T entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void Remove(T entity) => _dbSet.Remove(entity);
}
