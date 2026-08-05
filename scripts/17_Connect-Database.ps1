$root = "D:\Projects\Visual Studio\HJPlatform"


$apiProject = "$root\src\HJ.Server.Api\HJ.Server.Api.csproj"


dotnet add $apiProject reference `
"$root\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj"



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
"$root\src\HJ.Server.Api\Extensions\DatabaseExtensions.cs" `
-Encoding UTF8



Write-Host "Database extension created."