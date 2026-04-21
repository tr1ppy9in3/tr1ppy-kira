using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Kira.Security.Core.Entities;
using Kira.Security.Core.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Kira.Security.UseCases.Services;

public sealed class TokenGenerator(IOptions<TokenOptions> tokenOptions) 
{
    private readonly TokenOptions _tokenOptions = tokenOptions.Value; 
    
    public (string Token, string Jti) GenerateAccessToken(User user)
    {
        var jti = Guid.NewGuid().ToString();
        
        var claims = user.GetClaims().ToList();
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti)); 
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: claims,
            notBefore: null,
            expires: DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenLifetimeInMinutes),
            signingCredentials: credentials
        );
        
        return (new JwtSecurityTokenHandler().WriteToken(token), jti);
    }
    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}