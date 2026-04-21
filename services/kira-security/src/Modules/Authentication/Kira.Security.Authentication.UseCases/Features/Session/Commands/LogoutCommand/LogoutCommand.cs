using Kira.Security.Authentication.UseCases.Features.Session.Faults;
using Kira.UseCases.Results.Implementations;
using MediatR;

namespace Kira.Security.Authentication.UseCases.Features.Session.Commands.LogoutCommand;

public record LogoutCommand(string RefreshToken) : IRequest<Result<SessionFault>>;