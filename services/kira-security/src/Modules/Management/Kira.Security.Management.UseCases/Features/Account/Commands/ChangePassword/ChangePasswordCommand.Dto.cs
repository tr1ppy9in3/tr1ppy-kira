namespace Kira.Security.Management.UseCases.Features.Account.Commands.ChangePassword;

public record ChangePasswordCommandDto(string OldPassword, string NewPassword);