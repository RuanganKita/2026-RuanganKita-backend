using Microsoft.EntityFrameworkCore;
using RuanganKita.Api.Data;
using RuanganKita.Api.Dtos;
using RuanganKita.Api.Models;

namespace RuanganKita.Api.Services;

public class ReservationServices
{
    private readonly RuanganKitaContext _db;

    public ReservationServices(RuanganKitaContext db)
    {
        _db = db;
    }

    // GET ALL
    public async Task<List<ReservationDto>> GetAllAsync()
    {
        return await _db.Reservations
            .Include(r => r.Room)
            .Select(r => new ReservationDto(
                r.Id,
                r.RoomId,
                r.Room == null ? "No Name" : r.Room.Name,
                r.Room == null ? "No Name" : (r.Room.Building == null ? "No Building Specified" : r.Room.Building.Name),
                r.User == null ? -1 : r.User.Id,
                r.User == null ? "User not found" : r.User.Username,
                r.StartTime,
                r.EndTime,
                r.Status,
                r.CreatedAt,
                r.UpdatedAt
            ))
            .ToListAsync();
    }

    // GET BY ID
    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        var r = await _db.Reservations
            .Include(x => x.Room)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (r == null) return null;

        return new ReservationDto(
            r.Id,
            r.RoomId,
            r.Room == null ? "No Name" : r.Room.Name,
            r.Room == null ? "No Name" : (r.Room.Building == null ? "No Building Specified" : r.Room.Building.Name),
            r.User == null ? -1 : r.User.Id,
            r.User == null ? "User not found" : r.User.Username,
            r.StartTime,
            r.EndTime,
            r.Status,
            r.CreatedAt,
            r.UpdatedAt
        );
    }

        // CREATE
    public async Task<(bool Success, string? Error, ReservationDto? Reservation)> CreateAsync(CreateReservationDto dto, string username)
    {
        var room = await _db.Rooms
            .Include(r => r.Building)
            .FirstOrDefaultAsync(r => r.Id == dto.RoomId);

        if (room == null) return (false, "Room not found", null);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return (false, "User not found", null);

        var startDateTime = DateTime.SpecifyKind(dto.Date.ToDateTime(dto.StartTime), DateTimeKind.Utc);
        var endDateTime = DateTime.SpecifyKind(dto.Date.ToDateTime(dto.EndTime), DateTimeKind.Utc);

        if (dto.StartTime < room.AvailableFrom || dto.EndTime > room.AvailableTo)
            return (false, $"Reservation time must be between {room.AvailableFrom} and {room.AvailableTo}", null);

        var overlapping = await _db.Reservations.AnyAsync(r =>
            r.RoomId == dto.RoomId &&
            r.Status == "Approved" &&
            ((startDateTime >= r.StartTime && startDateTime < r.EndTime) ||
             (endDateTime > r.StartTime && endDateTime <= r.EndTime) ||
             (startDateTime <= r.StartTime && endDateTime >= r.EndTime))
        );

        if (overlapping)
            return (false, "Room already reserved at this time", null);

        var reservation = new Reservation
        {
            RoomId = dto.RoomId,
            UserId = user.Id,
            StartTime = startDateTime,
            EndTime = endDateTime,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync();

        return (true, null, new ReservationDto(
            reservation.Id,
            reservation.RoomId,
            room.Name,
            room.Building!.Name,
            reservation.UserId,
            user.Username,
            reservation.StartTime,
            reservation.EndTime,
            reservation.Status,
            reservation.CreatedAt,
            reservation.UpdatedAt
        ));
    }


    // UPDATE
    public async Task<(bool Success, string? Error, ReservationDto? Reservation)> UpdateAsync(int id, UpdateReservationDto dto, string username)
    {
        var res = await _db.Reservations
            .Include(r => r.Room)
            .ThenInclude(r => r!.Building)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (res == null)
            return (false, "Reservation not found", null);

        if (res.Room == null)
            return (false, "Room not found", null);

        if (res.User == null || res.User.Username != username)
            return (false, "You can only update your own reservations", null);

        if (res.Status == "Approved")
            return (false, "Cannot update a reservation that is already approved", null);

        var startDateTime = DateTime.SpecifyKind(dto.Date.ToDateTime(dto.StartTime), DateTimeKind.Utc);
        var endDateTime = DateTime.SpecifyKind(dto.Date.ToDateTime(dto.EndTime), DateTimeKind.Utc);

        if (dto.StartTime < res.Room.AvailableFrom || dto.EndTime > res.Room.AvailableTo)
            return (false, $"Reservation time must be between {res.Room.AvailableFrom} and {res.Room.AvailableTo}", null);

        var overlapping = await _db.Reservations.AnyAsync(r =>
            r.Id != id &&
            r.RoomId == res.RoomId &&
            r.Status == "Approved" &&
            ((startDateTime >= r.StartTime && startDateTime < r.EndTime) ||
             (endDateTime > r.StartTime && endDateTime <= r.EndTime) ||
             (startDateTime <= r.StartTime && endDateTime >= r.EndTime))
        );

        if (overlapping)
            return (false, "Room already reserved at this time", null);

        res.StartTime = startDateTime;
        res.EndTime = endDateTime;
        res.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return (true, null, new ReservationDto(
            res.Id,
            res.RoomId,
            res.Room!.Name,
            res.Room.Building!.Name,
            res.UserId,
            res.User!.Username,
            res.StartTime,
            res.EndTime,
            res.Status,
            res.CreatedAt,
            res.UpdatedAt
        ));
    }

    // DELETE
    public async Task<(bool Success, string? Error)> DeleteAsync(int id, string username)
    {
        var res = await _db.Reservations
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (res == null) 
            return (false, "Reservation not found");

        if (res.User == null || res.User.Username != username)
            return (false, "You can only delete your own reservations");

        if (res.Status == "Approved")
            return (false, "Cannot delete a reservation that is already approved");

        _db.Reservations.Remove(res);
        await _db.SaveChangesAsync();
        return (true, null);
    }

    // UPDATE STATUS RESERVATION (ADMIN)
    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        var res = await _db.Reservations
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (res == null) return false;

        if (status == "Approved")
        {
            var conflicting = await _db.Reservations
                .Where(r =>
                    r.Id != id &&
                    r.RoomId == res.RoomId &&
                    r.Status == "Pending" &&
                    r.StartTime < res.EndTime &&
                    r.EndTime > res.StartTime
                )
                .ToListAsync();

            if (conflicting.Any())
            {
                _db.Reservations.RemoveRange(conflicting);
            }
        }

        res.Status = status;
        res.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }


    // HISTORY RESERVATION BY USERNAME
    public async Task<List<ReservationDto>> GetHistoryAsync(string? username = null)
    {
        var query = _db.Reservations
            .Include(r => r.Room)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(username))
            query = query.Where(r => r.User!.Username == username);

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReservationDto(
                r.Id,
                r.RoomId,
                r.Room!.Name,
                r.Room.Building!.Name,
                r.UserId,
                r.User!.Username,
                r.StartTime,
                r.EndTime,
                r.Status,
                r.CreatedAt,
                r.UpdatedAt
            ))
            .ToListAsync();
    }

    // GET RESERVED HOURS
    public async Task<ReservedHoursResponseDto> GetReservedHoursAsync(DateOnly date, int roomId)
    {
        var startOfDay = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var endOfDay = date.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

        var reservations = await _db.Reservations
            .Where(r => r.RoomId == roomId &&
                        r.Status == "Approved" &&
                        r.StartTime >= startOfDay &&
                        r.EndTime <= endOfDay)
            .OrderBy(r => r.StartTime)
            .Select(r => new ReservedHourRangeDto(
                TimeOnly.FromDateTime(r.StartTime),
                TimeOnly.FromDateTime(r.EndTime)
            ))
            .ToListAsync();

        return new ReservedHoursResponseDto(date, roomId, reservations);
    }
}
