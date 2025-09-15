

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LicenseGate.API.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        var adminUsername = _configuration["Auth:AdminUsername"];
        var adminPassword = _configuration["Auth:AdminPassword"];

        if (username == adminUsername && password == adminPassword)
        {
            return GenerateJwtToken(username);
        }
        
        return null;
    }

    
    public string GenerateJwtToken(string username)
    {
        var key = _configuration["Auth:JwtSecret"];
        var issuer = _configuration["Auth:JwtIssuer"];

        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.ASCII.GetBytes(key);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = issuer,
            Audience = issuer,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature)

        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}