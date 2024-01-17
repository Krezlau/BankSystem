using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        return services;
    }
}