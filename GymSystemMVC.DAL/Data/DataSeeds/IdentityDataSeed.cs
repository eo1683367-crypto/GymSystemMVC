using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GymSystemMVC.DAL.Data.DataSeeds
{
    public static class IdentityDataSeed
    {
        public async static Task SeedAsync(RoleManager<IdentityRole> roleManager,
                                     UserManager<ApplicationUser> userManager,
                                     ILogger logger,
                                     CancellationToken ct = default)
        {

            try
            {
                bool hasUsers = userManager.Users.Any();
                bool hasRoles = roleManager.Roles.Any();

                if (hasUsers && hasRoles) return;

                if (!hasRoles)
                {
                    var roles = new List<IdentityRole>()
                    {
                        new IdentityRole(){Name = "SuperAdmin"},
                        new IdentityRole(){Name = "Admin"}
                    };

                    foreach (var roleName in roles.Select(R => R.Name))
                    {
                        if(!await roleManager.RoleExistsAsync(roleName))
                        {
                            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

                            if (!roleResult.Succeeded)
                                logger.LogError("Failed To Create Role...");
                        }
                    }
                }
                if (!hasUsers)
                {
                    // User 1 - SuperAdmin
                    var superAdminUser = new ApplicationUser
                    {
                        FirstName = "Islam",
                        LastName = "Omar",
                        UserName = "Soly",
                        Email = "Soly2004@gmail.com",
                        PhoneNumber = "01036272141",
                    };
                    var superAdminResult = await userManager.CreateAsync(superAdminUser, "P@ssw0rd");
                    if (superAdminResult.Succeeded)
                        await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");

                    // User 2 - Admin
                    var adminUser = new ApplicationUser
                    {
                        FirstName = "Ahmed",
                        LastName = "Ali",
                        UserName = "Ahmed",
                        Email = "ahmed@gym.com",
                        PhoneNumber = "01000000000",
                    };
                    var adminResult = await userManager.CreateAsync(adminUser, "P@ssw0rd");
                    if (adminResult.Succeeded)
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                return;
            }
            catch(Exception ex)
            {
                logger.LogError("Failed To Seed Identity Data");
                throw;
            }

        }
    }
}
