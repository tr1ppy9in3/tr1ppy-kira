using Kira.Security.Identity.UseCases.Features.RecoveryPassword.Faults;
using Kira.UseCases.CommandValidation;

namespace Kira.Security.Identity.UseCases.Features.RecoveryPassword.Commands.SendRecoveryPasswordCode;

public record SendRecoveryPasswordCodeCommand(string Email) : IValidatableCommand<RecoveryPasswordFault>;