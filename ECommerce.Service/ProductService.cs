using ECommerce.Core.DTOs.Products;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Service;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uow;
    public ProductService(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(ProductFilterDto filter)
    {
        // Generic repository'nin Query()'si üzerinden filtreli sorgu kurulur.
        var query = _uow.Repository<Product>().Query()
            .Include(p => p.Category)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(p => p.Name.Contains(filter.Search) || p.Description.Contains(filter.Search));

        if (filter.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);

        var list = await query.OrderBy(p => p.Name).ToListAsync();
        return list.Select(ToDto).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _uow.Repository<Product>().Query()
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        return product is null ? null : ToDto(product);
    }

    public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId
        };

        await _uow.Repository<Product>().AddAsync(product);
        await _uow.SaveChangesAsync();
        return (await GetByIdAsync(product.Id))!;
    }

    public async Task<ProductDto?> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var repo = _uow.Repository<Product>();
        var product = await repo.GetByIdAsync(id);
        if (product is null) return null;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.ImageUrl = dto.ImageUrl;
        product.CategoryId = dto.CategoryId;

        repo.Update(product);
        await _uow.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var repo = _uow.Repository<Product>();
        var product = await repo.GetByIdAsync(id);
        if (product is null) return false;

        repo.Remove(product);
        await _uow.SaveChangesAsync();
        return true;
    }

    private static ProductDto ToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        Stock = p.Stock,
        ImageUrl = p.ImageUrl,
        CategoryId = p.CategoryId,
        CategoryName = p.Category?.Name ?? string.Empty
    };
}
