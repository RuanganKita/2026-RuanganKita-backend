using System.ComponentModel.DataAnnotations;

namespace RuanganKita.Api.Dtos;

public record RegisterDto(
    [Required] string Username,
    [Required] string Password,
    string Role
);
