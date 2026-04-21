namespace Kira.Security.Authentication.UseCases.Features.Login.Commands;

public record LoginResponse(string AccessToken, string RefreshToken);