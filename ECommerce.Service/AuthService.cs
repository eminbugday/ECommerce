using ECommerce.Core.DTOs.Auth;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;

namespace ECommerce.Service;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenService _tokenService;

    public AuthService(IUnitOfWork uow, ITokenService tokenService)
    {
        _uow = uow;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var repo = _uow.Repository<User>();
        var existing = await repo.WhereAsync(u => u.Email == dto.Email);
        if (existing.Any())
            throw new InvalidOperationException("Bu e-posta zaten kayıtlı.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Customer
        };

        await repo.AddAsync(user);
        await _uow.SaveChangesAsync();
        return BuildResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var repo = _uow.Repository<User>();
        var user = (await repo.WhereAsync(u => u.Email == dto.Email)).FirstOrDefault();

        if (user is null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("E-posta veya şifre hatalı.");

        return BuildResponse(user);
    }

    public async Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var repo = _uow.Repository<User>();
        var user = (await repo.WhereAsync(u => u.Email == dto.Email)).FirstOrDefault();

        // Kullanıcı bulunamasa da aynı mesajı ver (güvenlik: email enumeration önleme)
        if (user is null)
            throw new InvalidOperationException("Bu e-posta ile kayıtlı hesap bulunamadı.");

        var code = new Random().Next(100000, 999999).ToString();
        user.PasswordResetToken = BCrypt.Net.BCrypt.HashPassword(code);
        user.PasswordResetExpiry = DateTime.UtcNow.AddMinutes(15);

        _uow.Repository<User>().Update(user);
        await _uow.SaveChangesAsync();

        return new ForgotPasswordResponseDto
        {
            Message = "Şifre sıfırlama kodu oluşturuldu. (Demo: kod direkt döndürülüyor, gerçekte e-posta ile gelir)",
            ResetCode = code
        };
    }

    public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var repo = _uow.Repository<User>();
        var user = (await repo.WhereAsync(u => u.Email == dto.Email)).FirstOrDefault();

        if (user is null || user.PasswordResetToken is null || user.PasswordResetExpiry is null)
            throw new InvalidOperationException("Geçersiz sıfırlama isteği.");

        if (user.PasswordResetExpiry < DateTime.UtcNow)
            throw new InvalidOperationException("Sıfırlama kodunun süresi dolmuş. Lütfen yeniden talep edin.");

        if (!BCrypt.Net.BCrypt.Verify(dto.ResetCode, user.PasswordResetToken))
            throw new InvalidOperationException("Girdiğiniz kod hatalı.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetExpiry = null;

        _uow.Repository<User>().Update(user);
        await _uow.SaveChangesAsync();

        return "Şifreniz başarıyla güncellendi. Yeni şifrenizle giriş yapabilirsiniz.";
    }

    private AuthResponseDto BuildResponse(User user)
    {
        var (token, expiresAt) = _tokenService.CreateToken(user);
        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}
