using BankSystem.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        return services;
    }

}