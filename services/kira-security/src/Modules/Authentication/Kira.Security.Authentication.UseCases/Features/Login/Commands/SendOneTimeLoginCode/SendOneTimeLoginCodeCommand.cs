using MediatR;

namespace Kira.Security.Authentication.UseCases.Features.Login.Commands.SendOneTimeLoginCode;

public record SendOneTimeLoginCodeCommand(string Email) : IRequest;