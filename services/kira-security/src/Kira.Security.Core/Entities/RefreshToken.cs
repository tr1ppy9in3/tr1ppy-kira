namespace Kira.Security.Core.Entities;

public class RefreshToken
{
    public required string Token { get; init; }
    public required string AccessTokenId { get; init; } 
    
    public required Guid UserId { get; init; }
    public DateTime ExpiresAt { get; init; }
} 