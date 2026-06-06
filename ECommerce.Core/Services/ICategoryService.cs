using ECommerce.Core.DTOs.Categories;

namespace ECommerce.Core.Services;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<CategoryDto> CreateAsync(CategoryCreateDto dto);
    Task<CategoryDto?> UpdateAsync(int id, CategoryCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
