using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BankSystem.Services.Auth;

public interface IJwtService
{
    string GenerateJwtToken(string email, Guid userId);
    
    Guid GetUserIdFromJwtToken(string token);
    
}

public class JwtService : IJwtService
{
    private readonly byte[] _secretKey;

    public JwtService(IConfiguration config)
    {
        _secretKey = Encoding.ASCII.GetBytes(config.GetSection("Key").Value);
    }

    public string GenerateJwtToken(string email, Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public Guid GetUserIdFromJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

        if (userIdClaim is null) throw new UnauthorizedAccessException("Invalid access token.");
        
        bool success = Guid.TryParse(userIdClaim.Value, out var userId);
        if (!success) throw new UnauthorizedAccessException("Invalid access token.");
        
        return userId;
    }
}