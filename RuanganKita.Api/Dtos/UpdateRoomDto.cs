using System.ComponentModel.DataAnnotations;

namespace RuanganKita.Api.Dtos;

public record UpdateRoomDto
(
    [Required] string Name,
    string Building,
    [Range (1, 1000)] int Capacity,
    TimeOnly AvailableFrom,
    TimeOnly AvailableTo
);