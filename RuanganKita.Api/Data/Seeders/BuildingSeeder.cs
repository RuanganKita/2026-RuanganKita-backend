using Microsoft.EntityFrameworkCore;
using RuanganKita.Api.Models;

namespace RuanganKita.Api.Data.Seeders;

public class BuildingSeeder : ISeeder
{
    private readonly RuanganKitaContext _context;

    public BuildingSeeder(RuanganKitaContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Buildings.AnyAsync())
        {
            Console.WriteLine("Buildings already seeded. Skipping...");
            return;
        }

        var buildings = new List<Building>
        {
            new Building { Name = "Gedung D3" },
            new Building { Name = "Gedung D4" },
            new Building { Name = "Gedung Pasca Sarjana" },
            new Building { Name = "Gedung SAW" }
        };

        _context.Buildings.AddRange(buildings);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Seeded {buildings.Count} buildings.");
    }
}
