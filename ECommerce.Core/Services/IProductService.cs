using ECommerce.Core.DTOs.Products;

namespace ECommerce.Core.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync(ProductFilterDto filter);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(ProductCreateDto dto);
    Task<ProductDto?> UpdateAsync(int id, ProductUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
