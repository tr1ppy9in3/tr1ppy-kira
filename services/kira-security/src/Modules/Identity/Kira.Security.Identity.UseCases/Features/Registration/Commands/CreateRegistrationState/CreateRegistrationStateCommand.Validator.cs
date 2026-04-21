using FluentValidation;
using Kira.Security.UseCases.Validation;

namespace Kira.Security.Identity.UseCases.Features.Registration.Commands.CreateRegistrationState;

/// <summary>
/// Валидатор для команды создания состояния регистрации.
/// </summary>
public sealed class CreateRegistrationStateCommandValidator : AbstractValidator<CreateRegistrationStateCommand>
{
    public CreateRegistrationStateCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Логин не может быть пустым")
            .MinimumLength(3).WithMessage("Логин должен содержать минимум 3 символа")
            .MaximumLength(50).WithMessage("Логин не должен превышать 50 символов")
            .Matches(@"^[a-zA-Z0-9_\.]+$").WithMessage("Логин может содержать только латинские буквы, цифры, точки и подчеркивания");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email не может быть пустым")
            .EmailAddress().WithMessage("Указан некорректный формат Email")
            .MaximumLength(150).WithMessage("Email не должен превышать 150 символов");
        
        RuleFor(x => x.Password).SetValidator(PasswordValidator.Create);
    }
}