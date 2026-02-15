using System;
using Microsoft.AspNetCore.Authorization;
using RuanganKita.Api.Dtos;
using RuanganKita.Api.Services;

namespace RuanganKita.Api.Endpoints;

public static class ReservationEndpoints
{
    public static RouteGroupBuilder MapReservationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/reservations");

        group.MapGet("/", async (ReservationServices service) =>
        {
            return Results.Ok(await service.GetAllAsync());
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        group.MapGet("/{id:int}", async (int id, ReservationServices service) =>
        {
            var res = await service.GetByIdAsync(id);
            return res is null ? Results.NotFound() : Results.Ok(res);
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        group.MapPost("/", async (CreateReservationDto dto, ReservationServices service, HttpContext context) =>
        {
            var username = context.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Results.Unauthorized();

            var (success, error, reservation) = await service.CreateAsync(dto, username);
            return success ? Results.Ok(reservation) : Results.BadRequest(error);
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        group.MapPut("/{id}", async (int id, UpdateReservationDto dto, ReservationServices service, HttpContext context) =>
        {
            var username = context.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Results.Unauthorized();

            var (success, error, reservation) = await service.UpdateAsync(id, dto, username);
            return success ? Results.Ok(reservation) : Results.BadRequest(error);
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        group.MapDelete("/{id}", async (int id, ReservationServices service, HttpContext context) =>
        {
            var username = context.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Results.Unauthorized();

            var (success, error) = await service.DeleteAsync(id, username);
            return success ? Results.Ok("Reservation deleted successfully") : Results.BadRequest(error);
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        group.MapPut("/status/{id:int}", async (int id, StatusUpdateDto dto, ReservationServices service) =>
        {
            var updated = await service.UpdateStatusAsync(id, dto.Status);
            return updated ? Results.Ok("Status updated") : Results.NotFound();
        })
        .RequireAuthorization("VerifiedAdmin");

        group.MapGet("/history", async (string? username, ReservationServices service) =>
        {
            return Results.Ok(await service.GetHistoryAsync(username));
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,User" });

        return group;
    }
}
