using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HJ.Server.Domain.Installations;
using HJ.Server.Domain.Products;
using HJ.Server.Infrastructure.Persistence;
using HJ.Server.Infrastructure.Persistence.Repositories;

namespace HJ.Server.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHJInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<HJDbContext>(options =>
            options.UseInMemoryDatabase("HJPlatformDb"));

        services.AddScoped<IInstallationRepository, InstallationRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}