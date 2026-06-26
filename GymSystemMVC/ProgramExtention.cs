using GymSystemMVC.DAL.Data.Contexts;
using GymSystemMVC.DAL.Data.DataSeeds;
using GymSystemMVC.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.PL
{
    public static class ProgramExtention
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app)
        {
            // Apply Migration + Seed

            using var scope = app.Services.CreateScope();

            var gymDbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>(); // DI Container
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>(); 
            var configurations = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Pending Migration

            var pendingMigration = await gymDbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigration.Any())
            {
                logger.LogInformation($"Apply {pendingMigration.Count()} pending Migrations...");
                await gymDbContext.Database.MigrateAsync();  // Apply Update DataBase...
            }
            // Seed

            var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot" , "Files");

            await GymDataSeed.SeedAsync(gymDbContext,seedPath,logger);
            await IdentityDataSeed.SeedAsync(RoleManager,UserManager,logger);
        }
    }
}
