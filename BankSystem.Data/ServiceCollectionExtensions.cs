using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBankDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<BankDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
}