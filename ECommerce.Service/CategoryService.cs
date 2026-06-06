using ECommerce.Core.DTOs.Categories;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;

namespace ECommerce.Service;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _uow;
    public CategoryService(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync()
    {
        var list = await _uow.Repository<Category>().GetAllAsync();
        return list.OrderBy(c => c.Name).Select(ToDto).ToList();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var c = await _uow.Repository<Category>().GetByIdAsync(id);
        return c is null ? null : ToDto(c);
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category { Name = dto.Name };
        await _uow.Repository<Category>().AddAsync(category);
        await _uow.SaveChangesAsync();
        return ToDto(category);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, CategoryCreateDto dto)
    {
        var repo = _uow.Repository<Category>();
        var category = await repo.GetByIdAsync(id);
        if (category is null) return null;

        category.Name = dto.Name;
        repo.Update(category);
        await _uow.SaveChangesAsync();
        return ToDto(category);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var repo = _uow.Repository<Category>();
        var category = await repo.GetByIdAsync(id);
        if (category is null) return false;

        repo.Remove(category);
        await _uow.SaveChangesAsync();
        return true;
    }

    private static CategoryDto ToDto(Category c) => new() { Id = c.Id, Name = c.Name };
}
