using System.Reflection;
using Kira.Security.Authentication.UseCases.Features.Login.Commands.LoginByCode;

namespace Kira.Security.Authentication.Bootstrap;

public static class AuthenticationAssembly
{
    public static Assembly Get => typeof(LoginByOneTimeCodeCommandHandler).Assembly;
    
}