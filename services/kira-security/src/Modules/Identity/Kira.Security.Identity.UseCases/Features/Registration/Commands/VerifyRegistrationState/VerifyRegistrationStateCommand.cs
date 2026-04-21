using Kira.Security.Identity.UseCases.Features.Registration.Faults;
using Kira.UseCases.Results.Implementations;
using MediatR;

namespace Kira.Security.Identity.UseCases.Features.Registration.Commands.VerifyRegistrationState;

public sealed record VerifyRegistrationStateCommand(
    string Email,
    string VerificationCode
) : IRequest<Result<RegistrationFault>>;