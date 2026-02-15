using Microsoft.EntityFrameworkCore;
using RuanganKita.Api.Models;

namespace RuanganKita.Api.Data;

public class RuanganKitaContext(DbContextOptions<RuanganKitaContext> options) : DbContext (options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Building> Buildings => Set<Building>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
}
