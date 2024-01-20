using System.Collections.Immutable;
using BankSystem.Services.Auth;
using BankSystem.Services.Transfers;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ITransferService, TransferService>();
        
        return services;
    }

}