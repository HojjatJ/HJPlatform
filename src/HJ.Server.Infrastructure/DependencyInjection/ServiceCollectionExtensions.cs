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
