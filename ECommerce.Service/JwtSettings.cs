namespace ECommerce.Service;

/// <summary>appsettings.json -> "Jwt" bölümüne karşılık gelir.</summary>
public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpireMinutes { get; set; } = 120;
}
