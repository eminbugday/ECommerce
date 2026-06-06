using ECommerce.Core.Entities;

namespace ECommerce.Core.Services;

/// <summary>JWT üretiminden tek sorumlu sınıf (Single Responsibility).</summary>
public interface ITokenService
{
    (string token, DateTime expiresAt) CreateToken(User user);
}
