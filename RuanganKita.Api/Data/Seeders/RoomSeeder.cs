using Microsoft.EntityFrameworkCore;
using RuanganKita.Api.Models;

namespace RuanganKita.Api.Data.Seeders;

public class RoomSeeder : ISeeder
{
    private readonly RuanganKitaContext _context;

    public RoomSeeder(RuanganKitaContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Rooms.AnyAsync())
        {
            Console.WriteLine("Rooms already seeded. Skipping...");
            return;
        }

        var buildings = await _context.Buildings.ToListAsync();
        if (!buildings.Any())
        {
            Console.WriteLine("No buildings found. Please seed buildings first.");
            return;
        }

        var rooms = new List<Room>
        {
            new Room
            {
                Name = "A301",
                BuildingId = buildings.First(b => b.Name == "Gedung D4").Id,
                Capacity = 30,
                AvailableFrom = new TimeOnly(8, 0),
                AvailableTo = new TimeOnly(17, 0)
            },
            new Room
            {
                Name = "A302",
                BuildingId = buildings.First(b => b.Name == "Gedung D4").Id,
                Capacity = 30,
                AvailableFrom = new TimeOnly(8, 0),
                AvailableTo = new TimeOnly(17, 0)
            },
            new Room
            {
                Name = "A303",
                BuildingId = buildings.First(b => b.Name == "Gedung D4").Id,
                Capacity = 30,
                AvailableFrom = new TimeOnly(8, 0),
                AvailableTo = new TimeOnly(17, 0)
            },
            
            new Room
            {
                Name = "HH101",
                BuildingId = buildings.First(b => b.Name == "Gedung D3").Id,
                Capacity = 30,
                AvailableFrom = new TimeOnly(7, 0),
                AvailableTo = new TimeOnly(18, 0)
            },
            new Room
            {
                Name = "HH102",
                BuildingId = buildings.First(b => b.Name == "Gedung D3").Id,
                Capacity = 30,
                AvailableFrom = new TimeOnly(8, 0),
                AvailableTo = new TimeOnly(16, 0)
            },
            
            new Room
            {
                Name = "10.10",
                BuildingId = buildings.First(b => b.Name == "Gedung SAW").Id,
                Capacity = 120,
                AvailableFrom = new TimeOnly(8, 0),
                AvailableTo = new TimeOnly(20, 0)
            },
            new Room
            {
                Name = "06.10",
                BuildingId = buildings.First(b => b.Name == "Gedung SAW").Id,
                Capacity = 120,
                AvailableFrom = new TimeOnly(8, 0),
                AvailableTo = new TimeOnly(20, 0)
            },
            
            new Room
            {
                Name = "06.06",
                BuildingId = buildings.First(b => b.Name == "Gedung SAW").Id,
                Capacity = 30,
                AvailableFrom = new TimeOnly(9, 0),
                AvailableTo = new TimeOnly(17, 0)
            },
            new Room
            {
                Name = "01.06",
                BuildingId = buildings.First(b => b.Name == "Gedung SAW").Id,
                Capacity = 30,
                AvailableFrom = new TimeOnly(8, 0),
                AvailableTo = new TimeOnly(16, 0)
            },
            
            new Room
            {
                Name = "05.05",
                BuildingId = buildings.First(b => b.Name == "Gedung Pasca Sarjana").Id,
                Capacity = 30,
                AvailableFrom = new TimeOnly(8, 0),
                AvailableTo = new TimeOnly(21, 0)
            },
            new Room
            {
                Name = "07.05",
                BuildingId = buildings.First(b => b.Name == "Gedung Pasca Sarjana").Id,
                Capacity = 30,
                AvailableFrom = new TimeOnly(8, 0),
                AvailableTo = new TimeOnly(21, 0)
            },
            new Room
            {
                Name = "05.07",
                BuildingId = buildings.First(b => b.Name == "Gedung Pasca Sarjana").Id,
                Capacity = 30,
                AvailableFrom = new TimeOnly(8, 0),
                AvailableTo = new TimeOnly(22, 0)
            }
        };

        _context.Rooms.AddRange(rooms);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Seeded {rooms.Count} rooms.");
    }
}
