namespace Kira.Security.Infrastructure.Options;

/// <summary>
/// Настройки SMTP сервера.
/// </summary>
public sealed class SmtpOptions
{
    /// <summary>
    /// Хост SMTP сервера 
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Порт 
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Использовать ли SSL/TLS.
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// Логин (почта отправителя).
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Пароль (или App Password для Yandex/Google).
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Имя отправителя, которое увидит пользователь.
    /// </summary>
    public string SenderName { get; set; } = "Kira Platform";
}