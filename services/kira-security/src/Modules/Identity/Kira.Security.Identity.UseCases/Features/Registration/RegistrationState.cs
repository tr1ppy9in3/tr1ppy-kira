using System.Diagnostics.CodeAnalysis;

namespace Kira.Security.Identity.UseCases.Features.Registration;

public record RegistrationState
{
    public static TimeSpan TimeToLive { get; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public required string Login { get; init; }
    
    /// <summary>
    /// Почта пользователя.
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// Хэш пароля.
    /// </summary>
    public required string PasswordHash { get; init; }
    
    /// <summary>
    /// Код запроса.
    /// </summary>
    public required string VerificationCode { get; init; }
    
    /// <summary>
    /// Время создания запроса.
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    
    /// <summary>
    /// Время истечения запроса.
    /// </summary>
    public DateTime ExpiredAt { get; private set; }

    /// <summary>
    /// Стандартный конструктор.
    /// </summary>
    [SetsRequiredMembers]
    public RegistrationState(string login, string email, string passwordHash, string verificationCode)
    {
        CreatedAt = DateTime.UtcNow;
        ExpiredAt = DateTime.UtcNow.Add(TimeToLive);
        
        Login = login;
        Email = email;
        PasswordHash = passwordHash;
        VerificationCode = verificationCode;
    }
};