using ECommerce.Core.DTOs.Users;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;

namespace ECommerce.Service;

/// <summary>Admin panelinin kullanıcı yönetimi mantığı.</summary>
public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;
    public UserService(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<UserDto>> GetAllAsync()
    {
        var users = await _uow.Repository<User>().GetAllAsync();
        return users.OrderByDescending(u => u.CreatedDate).Select(ToDto).ToList();
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _uow.Repository<User>().GetByIdAsync(id);
        return user is null ? null : ToDto(user);
    }

    public async Task<UserDto> CreateAsync(UserCreateDto dto)
    {
        var repo = _uow.Repository<User>();
        if ((await repo.WhereAsync(u => u.Email == dto.Email)).Any())
            throw new InvalidOperationException("Bu e-posta zaten kayıtlı.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = ParseRole(dto.Role)
        };

        await repo.AddAsync(user);
        await _uow.SaveChangesAsync();
        return ToDto(user);
    }

    public async Task<UserDto?> UpdateAsync(int id, UserUpdateDto dto)
    {
        var repo = _uow.Repository<User>();
        var user = await repo.GetByIdAsync(id);
        if (user is null) return null;

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.Role = ParseRole(dto.Role);
        user.IsActive = dto.IsActive;

        repo.Update(user);
        await _uow.SaveChangesAsync();
        return ToDto(user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var repo = _uow.Repository<User>();
        var user = await repo.GetByIdAsync(id);
        if (user is null) return false;

        repo.Remove(user);
        await _uow.SaveChangesAsync();
        return true;
    }

    private static UserRole ParseRole(string role) =>
        Enum.TryParse<UserRole>(role, true, out var r) ? r : UserRole.Customer;

    private static UserDto ToDto(User u) => new()
    {
        Id = u.Id,
        FullName = u.FullName,
        Email = u.Email,
        Role = u.Role.ToString(),
        IsActive = u.IsActive,
        CreatedDate = u.CreatedDate
    };
}
