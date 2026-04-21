using Kira.Security.Authentication.UseCases.Features.Login.Commands;
using Kira.Security.Authentication.UseCases.Features.Session.Faults;
using Kira.UseCases.Results.Implementations;
using MediatR;

namespace Kira.Security.Authentication.UseCases.Features.Session.Commands.RefreshCommand;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<SessionFault, LoginResponse>>;