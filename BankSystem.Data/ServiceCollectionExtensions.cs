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
            // options.UseNpgsql(connectionString);
            // options.UseSqlServer(
                // "Server=tcp:ochronadb.database.windows.net,1433;Initial Catalog=ochrona;Persist Security Info=False;User ID=ochrona;Password=Cebula123!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                options.UseSqlServer("Server=mssql1;Database=BankDB;User Id=SA;Password=sapassword123!@#;TrustServerCertificate=True;");
        });

        return services;
    }
}