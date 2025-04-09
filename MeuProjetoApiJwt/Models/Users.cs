namespace MeuProjetoApiJwt.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }

    public string Role { get; set; } = "User"; // Default role
    public bool CanDelete { get; set; } = false; // Default permission
}