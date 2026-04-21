using Kira.Security.Identity.UseCases.Features.RecoveryPassword.Faults;
using Kira.UseCases.CommandValidation;

namespace Kira.Security.Identity.UseCases.Features.RecoveryPassword.Commands.ResolveRecoveryPasswordCode;

public record ResolveRecoveryPasswordCodeCommand(
    string Email, 
    string Code, 
    string NewPassword
) : IValidatableCommand<RecoveryPasswordFault>;