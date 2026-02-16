using Microsoft.EntityFrameworkCore;
using RuanganKita.Api.Models;

namespace RuanganKita.Api.Data.Seeders;

public class ReservationSeeder : ISeeder
{
    private readonly RuanganKitaContext _context;

    public ReservationSeeder(RuanganKitaContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Reservations.AnyAsync())
        {
            Console.WriteLine("Reservations already seeded. Skipping...");
            return;
        }

        var users = await _context.Users.ToListAsync();
        var rooms = await _context.Rooms.ToListAsync();

        if (!users.Any() || !rooms.Any())
        {
            Console.WriteLine("No users or rooms found. Please seed them first.");
            return;
        }

        var today = DateTime.UtcNow.Date;
        var reservations = new List<Reservation>
        {
            // Reservasi hari ini
            new Reservation
            {
                RoomId = rooms[0].Id,
                UserId = users[1].Id,
                StartTime = DateTime.SpecifyKind(today.AddHours(9), DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(today.AddHours(11), DateTimeKind.Utc),
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            },
            new Reservation
            {
                RoomId = rooms[0].Id,
                UserId = users[2].Id,
                StartTime = DateTime.SpecifyKind(today.AddHours(13), DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(today.AddHours(15), DateTimeKind.Utc),
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            },
            new Reservation
            {
                RoomId = rooms[1].Id,
                UserId = users[3].Id,
                StartTime = DateTime.SpecifyKind(today.AddHours(10), DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(today.AddHours(12), DateTimeKind.Utc),
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            },
            
            // Reservasi besok
            new Reservation
            {
                RoomId = rooms[2].Id,
                UserId = users[1].Id,
                StartTime = DateTime.SpecifyKind(today.AddDays(1).AddHours(14), DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(today.AddDays(1).AddHours(16), DateTimeKind.Utc),
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            },
            new Reservation
            {
                RoomId = rooms[3].Id,
                UserId = users[2].Id,
                StartTime = DateTime.SpecifyKind(today.AddDays(1).AddHours(9), DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(today.AddDays(1).AddHours(11), DateTimeKind.Utc),
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            },
            
            // Reservasi kemarin (history)
            new Reservation
            {
                RoomId = rooms[0].Id,
                UserId = users[1].Id,
                StartTime = DateTime.SpecifyKind(today.AddDays(-1).AddHours(10), DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(today.AddDays(-1).AddHours(12), DateTimeKind.Utc),
                Status = "Approved",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Reservation
            {
                RoomId = rooms[1].Id,
                UserId = users[2].Id,
                StartTime = DateTime.SpecifyKind(today.AddDays(-2).AddHours(14), DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(today.AddDays(-2).AddHours(16), DateTimeKind.Utc),
                Status = "Rejected",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            }
        };

        _context.Reservations.AddRange(reservations);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Seeded {reservations.Count} reservations.");
    }
}
