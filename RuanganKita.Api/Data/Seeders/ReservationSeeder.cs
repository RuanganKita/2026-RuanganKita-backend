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

        var wibOffset = TimeSpan.FromHours(7);
        var nowUtc = DateTime.UtcNow;
        var nowWib = nowUtc.Add(wibOffset);
        var todayWib = DateOnly.FromDateTime(nowWib);
        
        DateTime CreateUtcDateTime(DateOnly dateWib, int hourWib)
        {
            var dateTimeWib = dateWib.ToDateTime(new TimeOnly(hourWib, 0));
            return DateTime.SpecifyKind(dateTimeWib.Subtract(wibOffset), DateTimeKind.Utc);
        }

        var reservations = new List<Reservation>
        {
            new Reservation
            {
                RoomId = rooms[0].Id,
                UserId = users[1].Id,
                StartTime = CreateUtcDateTime(todayWib, 9),
                EndTime = CreateUtcDateTime(todayWib, 11),
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            },
            new Reservation
            {
                RoomId = rooms[0].Id,
                UserId = users[2].Id,
                StartTime = CreateUtcDateTime(todayWib, 13),
                EndTime = CreateUtcDateTime(todayWib, 15),   
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            },
            new Reservation
            {
                RoomId = rooms[1].Id,
                UserId = users[3].Id,
                StartTime = CreateUtcDateTime(todayWib, 10), 
                EndTime = CreateUtcDateTime(todayWib, 12),    
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            },
            
            new Reservation
            {
                RoomId = rooms[2].Id,
                UserId = users[1].Id,
                StartTime = CreateUtcDateTime(todayWib.AddDays(1), 14),  
                EndTime = CreateUtcDateTime(todayWib.AddDays(1), 16),  
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            },
            new Reservation
            {
                RoomId = rooms[3].Id,
                UserId = users[2].Id,
                StartTime = CreateUtcDateTime(todayWib.AddDays(1), 9),  
                EndTime = CreateUtcDateTime(todayWib.AddDays(1), 11), 
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            },
            
            new Reservation
            {
                RoomId = rooms[0].Id,
                UserId = users[1].Id,
                StartTime = CreateUtcDateTime(todayWib.AddDays(-1), 10),
                EndTime = CreateUtcDateTime(todayWib.AddDays(-1), 12),   
                Status = "Approved",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Reservation
            {
                RoomId = rooms[1].Id,
                UserId = users[2].Id,
                StartTime = CreateUtcDateTime(todayWib.AddDays(-2), 14), 
                EndTime = CreateUtcDateTime(todayWib.AddDays(-2), 16),   
                Status = "Rejected",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            }
        };

        _context.Reservations.AddRange(reservations);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Seeded {reservations.Count} reservations.");
    }
}
