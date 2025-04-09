using MeuProjetoApiJwt.Data;
using MeuProjetoApiJwt.Models;
using MeuProjetoApiJwt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MeuProjetoApiJwt.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly TokenService _tokenService;

    public AuthController(DataContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            return BadRequest("Usuário já existe.");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = HashPassword(request.Password),
            Role =  request.Role.ToLower(),
            CanDelete = request.CanDelete
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Usuário registrado com sucesso!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized("Usuário ou senha inválidos.");

        var token = _tokenService.GenerateToken(user);
        return Ok(new { token });
    }

    private string HashPassword(string password)
    {
        var sha = SHA256.Create();
        var asByteArray = Encoding.Default.GetBytes(password);
        var hashed = sha.ComputeHash(asByteArray);
        return Convert.ToBase64String(hashed);
    }

    private bool VerifyPassword(string enteredPassword, string storedHash)
    {
        var enteredHash = HashPassword(enteredPassword);
        return storedHash == enteredHash;
    }
}

public class UserDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string Role { get; set; } = "User";
    public bool CanDelete { get; set; } = false; // Default permission
}