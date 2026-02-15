namespace RuanganKita.Api.Dtos;

public record UpdateReservationDto
(
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime
);
