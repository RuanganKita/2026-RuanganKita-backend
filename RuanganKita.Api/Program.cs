using Microsoft.EntityFrameworkCore;
using RuanganKita.Api.Data;
using RuanganKita.Api.Helper;
using RuanganKita.Api.Endpoints;
using RuanganKita.Api.Services;
using DotNetEnv;

// ENV LOAD
Env.Load();

// BUILDER
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RuanganKitaContext>(options =>
    options.UseNpgsql(ConnectionHelper.Build())
);

builder.Services.BuildJWT();

builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<RoomServices>();
builder.Services.AddScoped<ReservationServices>();
builder.Services.AddValidation();

// APP
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapRoomsEndpoints();
app.MapReservationEndpoints();

app.Run();
