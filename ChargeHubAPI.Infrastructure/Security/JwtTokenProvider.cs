using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChargeHubAPI.Application.Interfaces;
using ChargeHubAPI.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ChargeHubAPI.Infrastructure.Security;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtSettings _settings;

    public JwtTokenProvider(JwtSettings settings)
    {
        _settings = settings;
    }

    public string GenerateToken(User user)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId),
            new(ClaimTypes.Name, user.Name),
            new("username", user.Username),
            new("identecation", user.Identecation),
            new("phoneNumber", user.PhoneNumber),
            new(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

