$path = "src\HJ.Server.Infrastructure\DependencyInjection\ServiceCollectionExtensions.cs"

$content = @'
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HJ.Server.Infrastructure.Persistence;

namespace HJ.Server.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHJInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<HJDbContext>(options =>
        {
            options.UseSqlite(
                configuration.GetConnectionString("Default"));
        });

        return services;
    }
}
'@

Set-Content -Path $path -Value $content -Encoding UTF8