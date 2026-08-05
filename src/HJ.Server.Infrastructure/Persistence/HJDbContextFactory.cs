using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace HJ.Server.Infrastructure.Persistence;


public class HJDbContextFactory : IDesignTimeDbContextFactory<HJDbContext>
{
    public HJDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HJDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=hjplatform;Username=postgres;Password=postgres");


        return new HJDbContext(optionsBuilder.Options);
    }
}
