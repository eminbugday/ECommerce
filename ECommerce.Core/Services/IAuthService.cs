using ECommerce.Core.DTOs.Auth;

namespace ECommerce.Core.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<string> ResetPasswordAsync(ResetPasswordDto dto);
}
