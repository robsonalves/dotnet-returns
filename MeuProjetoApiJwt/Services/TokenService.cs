using MeuProjetoApiJwt.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MeuProjetoApiJwt.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var keyString = _configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(keyString))
            throw new InvalidOperationException("JWT Key não encontrada nas configurações.");

        var keyBytes = Encoding.UTF8.GetBytes(keyString);

        if (keyBytes.Length < 32)
            throw new ArgumentOutOfRangeException("Jwt:Key", $"A chave deve ter pelo menos 256 bits (32 caracteres ASCII). Tamanho atual: {keyBytes.Length * 8} bits");

        var key = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("canDelete", user.CanDelete ? "true" : "false"),
            new Claim(ClaimTypes.Role, user.Role ?? "user")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}