using Kira.Security.Identity.UseCases.Features.Registration.Faults;
using Kira.UseCases.CommandValidation;

namespace Kira.Security.Identity.UseCases.Features.Registration.Commands.CreateRegistrationState;

public sealed record CreateRegistrationStateCommand(
    string Login, 
    string Email, 
    string Password
) : IValidatableCommand<RegistrationFault> { }