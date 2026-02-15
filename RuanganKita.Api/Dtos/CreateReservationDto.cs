namespace RuanganKita.Api.Dtos;

public record CreateReservationDto
(
    int RoomId,
    int UserId,
    DateOnly Date,  
    TimeOnly StartTime,
    TimeOnly EndTime
);