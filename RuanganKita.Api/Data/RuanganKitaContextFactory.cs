using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RuanganKita.Api.Helper;
using DotNetEnv;

namespace RuanganKita.Api.Data;

public class RuanganKitaContextFactory 
    : IDesignTimeDbContextFactory<RuanganKitaContext>
{
    public RuanganKitaContext CreateDbContext(string[] args)
    {
        Env.Load();

        var optionsBuilder = new DbContextOptionsBuilder<RuanganKitaContext>();
        optionsBuilder.UseNpgsql(ConnectionHelper.Build());

        return new RuanganKitaContext(optionsBuilder.Options);
    }
}
