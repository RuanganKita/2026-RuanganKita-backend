# Database Seeders

This directory contains seeders for populating the database with initial data.

## Structure

- **ISeeder.cs** - Interface for all seeders
- **BuildingSeeder.cs** - Seeds building data
- **UserSeeder.cs** - Seeds user accounts with hashed passwords
- **RoomSeeder.cs** - Seeds rooms in various buildings
- **ReservationSeeder.cs** - Seeds sample reservations
- **DatabaseSeeder.cs** - Main orchestrator that runs all seeders in order
- **SeederExtensions.cs** - Extension method for easy integration

## Usage

### Option 1: Using Extension Method in Program.cs

Add this line in your `Program.cs` after building the app:

```csharp
await app.SeedDatabaseAsync();
```

Full example:
```csharp
var app = builder.Build();

// Seed database
await app.SeedDatabaseAsync();

app.Run();
```

### Option 2: Manual Seeding

You can also manually run the seeder:

```csharp
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<RuanganKitaContext>();
var seeder = new DatabaseSeeder(context);
await seeder.SeedAllAsync();
```

## Seeded Data

### Users
- **admin** / admin123 (Admin, Verified)
- **user1** / user123 (User, Verified)
- **user2** / user123 (User, Verified)
- **dosen1** / dosen123 (User, Verified)
- **mahasiswa1** / mahasiswa123 (User, Not Verified)

### Buildings
- Gedung A, B, C, D
- Gedung Teknik
- Gedung Fakultas Ekonomi
- Gedung Perpustakaan

### Rooms
12 rooms across different buildings with varying capacities and operating hours

### Reservations
Sample reservations for today, tomorrow, and historical data

## Notes

- Seeders are **idempotent** - they check if data exists before inserting
- Running the seeder multiple times is safe
- Seeders run in dependency order (Buildings → Users → Rooms → Reservations)
- Passwords are properly hashed using BCrypt
