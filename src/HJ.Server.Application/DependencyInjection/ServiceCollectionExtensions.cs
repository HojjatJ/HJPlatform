using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using HJ.Server.Application.Installations;
using HJ.Server.Application.Operations;
using HJ.Server.Application.Products;

namespace HJ.Server.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHJApplication(this IServiceCollection services)
    {
        services.AddScoped<InstallationMapper>();
        services.AddScoped<IInstallationService, InstallationService>();
        services.AddScoped<IOperationService, OperationService>();
        services.AddScoped<OperationMapper>();
        services.AddScoped<IProductService, ProductService>();
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }
}
