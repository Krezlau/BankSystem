using BankSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Api;

public static class DatabaseManagementService
{
    public static void MigrationInitialisation(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            serviceScope.ServiceProvider.GetService<BankDbContext>().Database.Migrate();
        }
    }
}
