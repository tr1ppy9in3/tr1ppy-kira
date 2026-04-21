using Kira.Security.Authentication.UseCases.Features.Login.Commands.Abstractions;

namespace Kira.Security.Authentication.UseCases.Features.Login.Commands.LoginByCode;

public record LoginByCodeCommand(
    string Email, 
    string Code
) : ILoginCommand;