namespace RuanganKita.Api.Dtos;

public record UserDto
(
    int Id,
    string Username,
    string Role,
    bool IsVerified
);