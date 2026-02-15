using System.ComponentModel.DataAnnotations;

namespace RuanganKita.Api.Dtos;

public record AuthResponseDto(
    [Required] string Token,
    int Id,
    [Required] string Username,
    [Required] string Role
);
