namespace RuanganKita.Api.Dtos;

public record RoomDto
(
    int Id,
    string Name,
    string Building,
    int Capacity,
    TimeOnly AvailableFrom,
    TimeOnly AvailableTo
);
