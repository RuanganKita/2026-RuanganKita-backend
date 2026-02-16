using Microsoft.EntityFrameworkCore;

namespace RuanganKita.Api.Data.Seeders;

public static class SeederExtensions
{
    public static async Task<IApplicationBuilder> SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RuanganKitaContext>();
        
        await context.Database.MigrateAsync();
        
        var seeder = new DatabaseSeeder(context);
        await seeder.SeedAllAsync();
        
        return app;
    }
}
