using Kira.Security.Authentication.UseCases.Features.Login.Commands.Abstractions;

namespace Kira.Security.Authentication.UseCases.Features.Login.Commands.LoginByLogin;

public record LoginByLoginCommand(
    string Login,
    string Password
) : ILoginCommand;