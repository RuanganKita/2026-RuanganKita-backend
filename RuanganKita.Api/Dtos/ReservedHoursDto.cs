namespace RuanganKita.Api.Dtos;

public record GetReservedHoursDto
(
    DateOnly Date,
    int RoomId
);

public record ReservedHourRangeDto
(
    TimeOnly StartTime,
    TimeOnly EndTime
);

public record ReservedHoursResponseDto
(
    DateOnly Date,
    int RoomId,
    List<ReservedHourRangeDto> ReservedHours
);
