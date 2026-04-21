namespace Kira.Security.Management.UseCases.Features.Account.Dtos;

public class UserProfileDto
{
    public string? Name { get; set; }
    public string? MiddleName { get; set; }
    public string? Surname { get; set; }
    public byte[]? ProfilePic { get; set; }
}