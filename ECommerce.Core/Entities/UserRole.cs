namespace ECommerce.Core.Entities;

/// <summary>
/// Üyelik rolleri. JWT içine bu rol yazılır ve [Authorize(Roles = ...)] ile kontrol edilir.
/// </summary>
public enum UserRole
{
    Customer = 0,
    Admin = 1
}
