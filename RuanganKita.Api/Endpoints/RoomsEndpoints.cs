using Microsoft.AspNetCore.Authorization;
using RuanganKita.Api.Dtos;
using RuanganKita.Api.Services;

namespace RuanganKita.Api.Endpoints;

public static class RoomsEndpoints
{
    public static RouteGroupBuilder MapRoomsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/rooms");

        group.MapGet("/", async (RoomServices service) =>
        {
            return Results.Ok(await service.GetAllAsync());
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        group.MapGet("/{id:int}", async (int id, RoomServices service) =>
        {
            var room = await service.GetByIdAsync(id);
            return room is null ? Results.NotFound() : Results.Ok(room);
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        group.MapPost("/", async (CreateRoomDto dto, RoomServices service) =>
        {
            var result = await service.CreateAsync(dto);

            if (!result.Success)
                return Results.BadRequest(result.Error);

            return Results.Created($"/rooms/{result.Data!.Id}", result.Data);
        })
        .RequireAuthorization("VerifiedAdmin");

        group.MapPut("/{id:int}", async (int id, CreateRoomDto dto, RoomServices service) =>
        {
            var result = await service.UpdateAsync(id, dto);

            if (!result.Success)
                return result.Error == "Room not found."
                    ? Results.NotFound()
                    : Results.BadRequest(result.Error);

            return Results.Ok(result.Data);
        })
        .RequireAuthorization("VerifiedAdmin");

        group.MapDelete("/{id:int}", async (int id, RoomServices service) =>
        {
            return await service.DeleteAsync(id)
                ? Results.NoContent()
                : Results.NotFound();
        })
        .RequireAuthorization("VerifiedAdmin");

        return group;
    }
}