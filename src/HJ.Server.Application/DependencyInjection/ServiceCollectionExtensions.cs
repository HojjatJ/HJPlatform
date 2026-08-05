using Microsoft.Extensions.DependencyInjection;

namespace HJ.Server.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHJApplication(
        this IServiceCollection services)
    {
        services.AddScoped<Products.IProductService, Products.ProductService>();

        return services;
    }
}
