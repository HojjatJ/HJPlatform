using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HJ.Server.Domain.Installations;
using HJ.Server.Domain.Operations;
using HJ.Server.Domain.Products;
using HJ.Server.Foundation.Abstractions.Logging;
using HJ.Server.Foundation.Abstractions.Telemetry;
using HJ.Server.Infrastructure.Logging;
using HJ.Server.Infrastructure.Persistence;
using HJ.Server.Infrastructure.Persistence.Repositories;
using HJ.Server.Infrastructure.Telemetry;

namespace HJ.Server.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHJInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<HJDbContext>(options =>
                options.UseNpgsql(connectionString));
        }
        else
        {
            services.AddDbContext<HJDbContext>(options =>
                options.UseInMemoryDatabase("HJPlatformDb"));
        }

        services.AddScoped<IInstallationRepository, InstallationRepository>();
        services.AddScoped<IOperationRepository, OperationRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITelemetryService, TelemetryService>();
        services.AddScoped<ILoggingService, LoggingService>();

        return services;
    }
}