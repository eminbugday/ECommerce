using ECommerce.Core.DTOs.Users;

namespace ECommerce.Core.Services;

/// <summary>Admin panelindeki kullanıcı yönetimi (listele / ekle / düzenle / sil).</summary>
public interface IUserService
{
    Task<IReadOnlyList<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(UserCreateDto dto);
    Task<UserDto?> UpdateAsync(int id, UserUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
