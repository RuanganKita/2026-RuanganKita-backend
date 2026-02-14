using Microsoft.EntityFrameworkCore;
using RuanganKita.Api.Data;
using RuanganKita.Api.Dtos;
using RuanganKita.Api.Models;

namespace RuanganKita.Api.Services;

public class RoomServices
{
    private readonly RuanganKitaContext _db;

    public RoomServices(RuanganKitaContext db)
    {
        _db = db;
    }

    // GET ALL
    public async Task<List<RoomDto>> GetAllAsync()
    {
        return await _db.Rooms
            .Include(r => r.Building)
            .Select(r => new RoomDto(
                r.Id,
                r.Name,
                r.Building != null ? r.Building.Name : "No Building Specified",
                r.Capacity,
                r.AvailableFrom,
                r.AvailableTo
            ))
            .ToListAsync();
    }

    // GET BY ID
    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        var room = await _db.Rooms
            .Include(r => r.Building)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room is null)
            return null;

        return new RoomDto(
            room.Id,
            room.Name,
            room.Building != null ? room.Building.Name : "No Building Specified",
            room.Capacity,
            room.AvailableFrom,
            room.AvailableTo
        );
    }

    // CREATE
    public async Task<(bool Success, string? Error, RoomDto? Data)> CreateAsync(CreateRoomDto dto)
    {
        if (await RoomExistsInSameBuilding(dto.Name, dto.Building))
            return (false, $"Room '{dto.Name}' already exists in that building.", null);

        var building = await GetOrCreateBuildingAsync(dto.Building);

        var room = new Room
        {
            Name = dto.Name.Trim(),
            Building = building,
            Capacity = dto.Capacity,
            AvailableFrom = dto.AvailableFrom,
            AvailableTo = dto.AvailableTo
        };

        _db.Rooms.Add(room);
        await _db.SaveChangesAsync();

        return (true, null, new RoomDto(
            room.Id,
            room.Name,
            building?.Name ?? "No Building Specified",
            room.Capacity,
            room.AvailableFrom,
            room.AvailableTo
        ));
    }

    // UPDATE
    public async Task<(bool Success, string? Error, RoomDto? Data)> UpdateAsync(int id, CreateRoomDto dto)
    {
        var room = await _db.Rooms
            .Include(r => r.Building)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room is null)
            return (false, "Room not found.", null);

        if (await RoomExistsInSameBuilding(dto.Name, dto.Building, id))
            return (false, $"Room '{dto.Name}' already exists in that building.", null);

        var building = await GetOrCreateBuildingAsync(dto.Building);

        room.Name = dto.Name.Trim();
        room.Building = building;
        room.Capacity = dto.Capacity;
        room.AvailableFrom = dto.AvailableFrom;
        room.AvailableTo = dto.AvailableTo;

        await _db.SaveChangesAsync();

        return (true, null, new RoomDto(
            room.Id,
            room.Name,
            building?.Name ?? "No Building Specified",
            room.Capacity,
            room.AvailableFrom,
            room.AvailableTo
        ));
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        var room = await _db.Rooms.FindAsync(id);
        if (room is null)
            return false;

        _db.Rooms.Remove(room);
        await _db.SaveChangesAsync();
        return true;
    }

    // HELPERS

    private async Task<bool> RoomExistsInSameBuilding(
        string roomName,
        string? buildingName,
        int? excludeRoomId = null)
    {
        var normalizedRoom = roomName.Trim().ToLower();
        var normalizedBuilding = buildingName?.Trim().ToLower();

        return await _db.Rooms
            .Include(r => r.Building)
            .AnyAsync(r =>
                (excludeRoomId == null || r.Id != excludeRoomId) &&
                r.Name.ToLower() == normalizedRoom &&
                (
                    (r.Building == null && normalizedBuilding == null) ||
                    (r.Building != null &&
                     r.Building.Name.ToLower() == normalizedBuilding)
                )
            );
    }

    private async Task<Building?> GetOrCreateBuildingAsync(string? buildingName)
    {
        if (string.IsNullOrWhiteSpace(buildingName))
            return null;

        var normalizedName = buildingName.Trim().ToLower();

        var building = await _db.Buildings
            .FirstOrDefaultAsync(b =>
                b.Name.ToLower() == normalizedName);

        if (building is not null)
            return building;

        building = new Building
        {
            Name = buildingName.Trim()
        };

        _db.Buildings.Add(building);
        return building;
    }
}
