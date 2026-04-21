using FluentValidation;

namespace Kira.Security.UseCases.Validation;

/// <summary>
/// Изолированный валидатор для проверки сложности пароля.
/// </summary>
public sealed class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Пароль не может быть пустым")
            .MinimumLength(8).WithMessage("Пароль должен содержать не менее 8 символов")
            .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
            .Matches(@"[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру");
    }
    
    public static PasswordValidator Create => new PasswordValidator();
}