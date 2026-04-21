namespace Kira.Security.UseCases.Abstractions;

public interface IClientInfoProvider
{
    string GetIpAddress();
    string GetUserAgent();
}