using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infrastructure.Repository;

namespace ProductManagement.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        return services;
    }
}
