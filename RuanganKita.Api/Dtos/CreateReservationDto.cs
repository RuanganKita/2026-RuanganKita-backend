namespace RuanganKita.Api.Dtos;

public record CreateReservationDto
(
    int RoomId,
    DateOnly Date,  
    TimeOnly StartTime,
    TimeOnly EndTime
);