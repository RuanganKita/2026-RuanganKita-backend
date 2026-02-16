using Microsoft.EntityFrameworkCore;
using RuanganKita.Api.Data;
using RuanganKita.Api.Dtos;

namespace RuanganKita.Api.Services;

public class UserServices
{
    private readonly RuanganKitaContext _db;

    public UserServices(RuanganKitaContext db)
    {
        _db = db;
    }

    // GET ALL
    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _db.Users
            .Select(u => new UserDto(
                u.Id,
                u.Username,
                u.Role,
                u.IsVerified
            ))
            .ToListAsync();
    }

    // GET BY ID
    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(r => r.Id == id);

        if (user is null)
            return null;

        return new UserDto(
            user.Id,
            user.Username,
            user.Role,
            user.IsVerified
        );
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null)
            return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }
}
