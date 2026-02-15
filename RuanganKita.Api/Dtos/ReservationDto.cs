namespace RuanganKita.Api.Dtos;

public record ReservationDto
(
    int Id,
    int RoomId,
    string RoomName,
    string BuildingName,
    int UserId,
    string Username,
    DateTime StartTime,
    DateTime EndTime,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);