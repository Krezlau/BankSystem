using BankSystem.Repositories.Users;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}