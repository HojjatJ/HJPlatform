$root = "D:\Projects\Visual Studio\HJPlatform"

$extensionPath = "$root\src\HJ.Server.Api\Extensions"

if(!(Test-Path $extensionPath))
{
    New-Item -ItemType Directory -Path $extensionPath | Out-Null
}


@"
using HJ.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HJ.Server.Api.Extensions;


public static class DatabaseExtensions
{
    public static IServiceCollection AddHJDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<HJDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("Default"));
        });

        return services;
    }
}
"@ | Set-Content `
"$extensionPath\DatabaseExtensions.cs" `
-Encoding UTF8


Write-Host "Database extension patched successfully."