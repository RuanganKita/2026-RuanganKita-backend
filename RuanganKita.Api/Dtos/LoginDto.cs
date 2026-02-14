using System.ComponentModel.DataAnnotations;

namespace RuanganKita.Api.Dtos;

public record LoginDto(
    [Required] string Username,
    [Required] string Password
);
