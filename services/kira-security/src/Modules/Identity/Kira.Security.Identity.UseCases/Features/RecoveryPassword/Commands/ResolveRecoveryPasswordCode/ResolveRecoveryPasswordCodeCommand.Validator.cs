using FluentValidation;
using Kira.Security.UseCases.Validation;

namespace Kira.Security.Identity.UseCases.Features.RecoveryPassword.Commands.ResolveRecoveryPasswordCode;

public sealed class ResolveRecoveryPasswordCodeCommandValidator : AbstractValidator<ResolveRecoveryPasswordCodeCommand>
{
    public ResolveRecoveryPasswordCodeCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .SetValidator(PasswordValidator.Create);
    }
}