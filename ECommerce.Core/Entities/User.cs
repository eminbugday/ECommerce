namespace ECommerce.Core.Entities;

/// <summary>
/// Üye / kullanıcı. Admin panelinde listelenir, düzenlenir (kalem butonu) ve silinir.
/// </summary>
public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Customer;
    public bool IsActive { get; set; } = true;

    // Bir kullanıcının verdiği siparişler
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
