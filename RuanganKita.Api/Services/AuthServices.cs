using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RuanganKita.Api.Data;
using RuanganKita.Api.Dtos;
using RuanganKita.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RuanganKita.Api.Services;

public class AuthServices
{
    private readonly RuanganKitaContext _db;

    public AuthServices(RuanganKitaContext db)
    {
        _db = db;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user == null)
            return null;

        if (user.Role == "Admin" && !user.IsVerified)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        var token = GenerateJwtToken(user);

        return new AuthResponseDto(token, user.Username, user.Role);
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        var exists = await _db.Users
            .AnyAsync(u => u.Username == dto.Username);

        if (exists)
            return false;

        var role = dto.Role == "Admin" ? "Admin" : "User";

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role,
            IsVerified = role == "Admin" ? false : true
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return true;
    }


    private string GenerateJwtToken(User user)
    {
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        var jwtDuration = Environment.GetEnvironmentVariable("JWT_DURATION_MINUTES");

        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new Exception("JWT_KEY is missing.");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        );

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("isVerified", user.IsVerified.ToString().ToLower())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(jwtDuration ?? "60")
            ),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
