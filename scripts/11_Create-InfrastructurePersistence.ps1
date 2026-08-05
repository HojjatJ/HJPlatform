$root = "D:\Projects\Visual Studio\HJPlatform"

$project = "$root\src\HJ.Server.Infrastructure"


$folders = @(
    "$project\Persistence",
    "$project\DependencyInjection"
)


foreach ($folder in $folders)
{
    if (!(Test-Path $folder))
    {
        New-Item -ItemType Directory -Path $folder | Out-Null
        Write-Host "Created: $folder"
    }
}


# DbContext

$dbContext = @"
using Microsoft.EntityFrameworkCore;

namespace HJ.Server.Infrastructure.Persistence;

public class HJServerDbContext : DbContext
{
    public HJServerDbContext(
        DbContextOptions<HJServerDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
"@


Set-Content `
    -Path "$project\Persistence\HJServerDbContext.cs" `
    -Value $dbContext `
    -Encoding UTF8



# DI Extension

$di = @"
using HJ.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HJ.Server.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHJInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<HJServerDbContext>(options =>
        {
            var connectionString =
                configuration.GetConnectionString("Default");

            options.UseNpgsql(connectionString);
        });


        return services;
    }
}
"@


Set-Content `
    -Path "$project\DependencyInjection\ServiceCollectionExtensions.cs" `
    -Value $di `
    -Encoding UTF8



Write-Host ""
Write-Host "Infrastructure persistence created."