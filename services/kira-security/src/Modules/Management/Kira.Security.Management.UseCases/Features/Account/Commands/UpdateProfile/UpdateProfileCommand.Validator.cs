using FluentValidation;

namespace Kira.Security.Management.UseCases.Features.Account.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.Model.Name)
            .MaximumLength(64)
            .NotEmpty()
            .WithMessage("Имя не должно превышать 64 символов!");
        
        RuleFor(x => x.Model.Surname)
            .MaximumLength(64)
            .NotEmpty()
            .WithMessage("Фамилия не должна превышать 64 символов!");

        RuleFor(x => x.Model.MiddleName)
            .MaximumLength(64)
            .NotEmpty()
            .WithMessage("Отчество не должно превышать 64 символов!");
        
        RuleFor(x => x.Model.ProfilePic)
            .Must(x => x == null || x.Length <= 5 * 1024 * 1024)
            .WithMessage("Размер фотографии не должен превышать 5 МБ.");
    }
}