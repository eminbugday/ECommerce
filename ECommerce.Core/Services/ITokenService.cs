using ECommerce.Core.Entities;

namespace ECommerce.Core.Services;

public interface ITokenService
{
    (string token, DateTime expiresAt) CreateToken(User user);
}
