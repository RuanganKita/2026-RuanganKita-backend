using System.ComponentModel.DataAnnotations;

namespace RuanganKita.Api.Models;

public class User
{
    public int Id { get; set; }

    public required string Username { get; set; } = null!;

    public required string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = "User"; 
    
    public bool IsVerified { get; set; } = false;
}
