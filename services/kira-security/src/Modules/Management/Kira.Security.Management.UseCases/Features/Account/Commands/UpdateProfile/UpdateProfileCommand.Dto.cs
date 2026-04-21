namespace Kira.Security.Management.UseCases.Features.Account.Commands.UpdateProfile;

/// <summary>
/// Данные для обновления профиля пользователя.
/// </summary>
public record UpdateProfileCommandDto(
    string? Name, 
    string? Surname, 
    string? MiddleName, 
    byte[]? ProfilePic
);