using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.DTOs.Users;

/// <summary>Admin panelinde listelenen kullanıcı satırı (şifre dönülmez).</summary>
public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>Admin yeni kullanıcı eklerken.</summary>
public class UserCreateDto
{
    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    /// <summary>"Admin" veya "Customer".</summary>
    public string Role { get; set; } = "Customer";
}

/// <summary>Admin kalem (düzenle) butonuyla kullanıcı güncellerken.</summary>
public class UserUpdateDto
{
    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = "Customer";
    public bool IsActive { get; set; } = true;
}
