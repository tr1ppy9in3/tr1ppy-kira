namespace Kira.Security.Management.Core.Entities;

public sealed class UserProfile
{
    public Guid UserId { get; set; }
    public string? Name { get; set; }
    public string? MiddleName { get; set; }
    public string? Surname { get; set; }
    public byte[]? ProfilePic { get; set; }
}