using Kira.Security.Authentication.UseCases.Features.Login.Commands.Abstractions;

namespace Kira.Security.Authentication.UseCases.Features.Login.Commands.LoginByEmail;

public record LoginByEmailCommand(
    string Email,
    string Password
) : ILoginCommand;