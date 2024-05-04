using Application.Infrastructure.Models.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Infrastructure.Helpers;

public static class TokenGenerator
{
    public static string GenerateJwtToken(this IEnumerable<Claim> claims, JwtConfiguration jwtConfiguration)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey));

        var token = new JwtSecurityToken
           (audience: jwtConfiguration.Audience,
            issuer: jwtConfiguration.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtConfiguration.TokenValidityInMinutes),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
