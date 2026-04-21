using FluentValidation;
using Kira.Security.UseCases.Validation;

namespace Kira.Security.Management.UseCases.Features.Account.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Model.NewPassword)
            .SetValidator(PasswordValidator.Create);
    }
}