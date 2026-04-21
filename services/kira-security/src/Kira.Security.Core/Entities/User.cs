using System.Security.Claims;
using Kira.Security.Core.Options;
using Kira.Security.Core.Services;

namespace Kira.Security.Core.Entities;

public sealed class User
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();
    
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public required string Login { get; set; }
    
    /// <summary>
    /// Хэш пароля пользователя.
    /// </summary>
    public required string PasswordHash { get; set; } 
    
    /// <summary>
    /// Почта пользователя.
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Заблокирован (да, нет)
    /// </summary>
    public bool IsBlocked { get; set; } = false;
    
    // /// <summary>
    // /// Профиль пользователя.
    // /// </summary>
    // public required UserProfile Profile { get; init; }


    /// <summary>
    /// Получить клэймы.
    /// </summary>
    /// <returns> Массив клэймов. </returns>
    public Claim[] GetClaims()
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, Login),
            new(ClaimTypes.NameIdentifier, Id.ToString()),
            new(ClaimTypes.Email, Email),
        };
        
        return claims.ToArray();
    }
    
    /// <summary>
    /// Поставить новый пароль пользователю.
    /// </summary>
    /// <param name="password"> Строка пароля.</param>
    /// <param name="passwordOptions"> Настройки генерации хэша для пароля. </param>
    public void SetPassword(string password, PasswordOptions passwordOptions)
    {
        PasswordHash = CryptographyService.HashPassword(password, passwordOptions.Salt);
    }
}