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
