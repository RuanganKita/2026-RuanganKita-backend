using Microsoft.EntityFrameworkCore;
using RuanganKita.Api.Models;

namespace RuanganKita.Api.Data.Seeders;

public class UserSeeder : ISeeder
{
    private readonly RuanganKitaContext _context;

    public UserSeeder(RuanganKitaContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Users.AnyAsync())
        {
            Console.WriteLine("Users already seeded. Skipping...");
            return;
        }

        var users = new List<User>
        {
            new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin",
                IsVerified = true
            },
            new User
            {
                Username = "user1",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                Role = "User",
                IsVerified = true
            },
            new User
            {
                Username = "user2",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                Role = "User",
                IsVerified = true
            },
            new User
            {
                Username = "dosen1",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("dosen123"),
                Role = "User",
                IsVerified = true
            },
            new User
            {
                Username = "mahasiswa1",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("mahasiswa123"),
                Role = "User",
                IsVerified = true
            }
        };

        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Seeded {users.Count} users.");
    }
}
