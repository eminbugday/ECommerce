using System.Collections.Concurrent;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;

namespace ECommerce.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext context) => _context = context;

    public IGenericRepository<T> Repository<T>() where T : BaseEntity =>
        (IGenericRepository<T>)_repositories.GetOrAdd(typeof(T),
            _ => new GenericRepository<T>(_context));

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
