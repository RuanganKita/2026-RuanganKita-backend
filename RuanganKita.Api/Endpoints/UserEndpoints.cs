using RuanganKita.Api.Data;
using RuanganKita.Api.Dtos;
using RuanganKita.Api.Services;

namespace RuanganKita.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");

        group.MapGet("/", async (UserServices service) => 
        {
            return Results.Ok(await service.GetAllAsync());
        })
        .RequireAuthorization("VerifiedAdmin");

        group.MapGet("/{id:int}", async (int id, UserServices service) =>
        {
            var user = await service.GetByIdAsync(id);
            return user is null ? Results.NotFound() : Results.Ok(user);
        })
        .RequireAuthorization("VerifiedAdmin");

        group.MapDelete("/{id:int}", async (int id, UserServices service) =>
        {
            return await service.DeleteAsync(id)
                ? Results.NoContent()
                : Results.NotFound();
        })
        .RequireAuthorization("VerifiedAdmin");

        return group;
    }
}