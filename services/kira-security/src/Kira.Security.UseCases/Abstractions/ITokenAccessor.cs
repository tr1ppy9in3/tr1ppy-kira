namespace Kira.Security.UseCases.Abstractions;

public interface ITokenAccessor
{
    string? GetCurrentAccessToken();
}