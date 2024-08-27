using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace MosadApiServer.Servises;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public string CreateToken(string name)
    {
        string? key = configuration.GetValue<string>("Jwt:Key", null)
                      ?? throw new ArgumentNullException("Invalid JWT key configuration");

        int expiration = configuration.GetValue("Jwt:Expiry", 60);
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims = [new(ClaimTypes.Name, name)];
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(expiration),
            claims: claims,
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}