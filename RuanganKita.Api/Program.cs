using Microsoft.EntityFrameworkCore;
using RuanganKita.Api.Data;
using RuanganKita.Api.Helper;
using RuanganKita.Api.Endpoints;
using RuanganKita.Api.Services;
using DotNetEnv;
using RuanganKita.Api.Data.Seeders;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RuanganKitaContext>(options =>
    options.UseNpgsql(ConnectionHelper.Build())
);
builder.Services.BuildJWT();
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<RoomServices>();
builder.Services.AddScoped<ReservationServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();


var app = builder.Build();

app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapAuthEndpoints();
app.MapRoomsEndpoints();
app.MapReservationEndpoints();
app.MapUserEndpoints();

await app.SeedDatabaseAsync();

app.Run();
