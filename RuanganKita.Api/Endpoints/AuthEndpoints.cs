using RuanganKita.Api.Data;
using RuanganKita.Api.Dtos;
using RuanganKita.Api.Services;

namespace RuanganKita.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/register", async (
            RegisterDto dto,
            AuthServices service) =>
        {
            var result = await service.RegisterAsync(dto);

            if (!result)
                return Results.BadRequest("Username already exists");

            return Results.Ok("User registered");
        });

        group.MapPost("/login", async (
            LoginDto dto,
            AuthServices service) =>
        {
            var result = await service.LoginAsync(dto);

            if (result == null)
                return Results.Unauthorized();

            return Results.Ok(result);
        });

        group.MapPost("/verify/{id:int}", async (
            int id,
            RuanganKitaContext db
        ) =>
        {
            var user = await db.Users.FindAsync(id);

            if (user == null)
                return Results.NotFound();

            if (user.Role != "Admin")
                return Results.BadRequest("User is not an admin.");

            user.IsVerified = true;
            await db.SaveChangesAsync();

            return Results.Ok("Admin verified.");
        })
        .RequireAuthorization("VerifiedAdmin");


        return group;
    }
}