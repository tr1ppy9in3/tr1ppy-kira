namespace Kira.Security.Core.Options;

/// <summary>
/// Настройки JWT токена
/// </summary>
public class TokenOptions
{
    /// <summary>
    /// Секретный ключ для генерации JWT токена 
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Издатель
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Получатель
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Время жизни Access токена в минутах
    /// </summary>
    public int AccessTokenLifetimeInMinutes { get; set; }

    /// <summary>
    /// Время жизни Refresh токена в минутах
    /// </summary>
    public int RefreshTokenLifetimeInMinutes { get; set; }
}