namespace RuanganKita.Api.Data.Seeders;

public class DatabaseSeeder
{
    private readonly RuanganKitaContext _context;

    public DatabaseSeeder(RuanganKitaContext context)
    {
        _context = context;
    }

    public async Task SeedAllAsync()
    {
        Console.WriteLine("Starting database seeding...");
        Console.WriteLine("======================================");

        try
        {
            var buildingSeeder = new BuildingSeeder(_context);
            await buildingSeeder.SeedAsync();

            var userSeeder = new UserSeeder(_context);
            await userSeeder.SeedAsync();

            var roomSeeder = new RoomSeeder(_context);
            await roomSeeder.SeedAsync();

            var reservationSeeder = new ReservationSeeder(_context);
            await reservationSeeder.SeedAsync();

            Console.WriteLine("Database seeding completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during seeding: {ex.Message}");
            throw;
        }
    }
}
