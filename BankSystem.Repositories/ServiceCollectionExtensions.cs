using BankSystem.Repositories.Auth;
using BankSystem.Repositories.Transfers;
using BankSystem.Repositories.Users;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILoginRequestRepository, LoginRequestRepository>();
        services.AddScoped<ITransferRepository, TransferRepository>();
        
        return services;
    }
}