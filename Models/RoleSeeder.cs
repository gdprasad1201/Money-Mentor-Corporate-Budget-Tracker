using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Expense_Tracker.Models
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Define roles to be created
            string[] roleNames = { "Admin", "User" };

            // Create roles if they do not exist
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Define admin user email
            const string adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // Create or update admin user
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true // Ensure the email is confirmed
                };

                var creationResult = await userManager.CreateAsync(adminUser, "AdminPassword123!"); // Ensure this meets your password policy

                if (creationResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    // Log errors during user creation
                    foreach (var error in creationResult.Errors)
                    {
                        Console.WriteLine($"Error creating admin user: {error.Description}");
                    }
                }
            }
            else
            {
                // Update existing admin user details if necessary
                adminUser.EmailConfirmed = true; // Ensure email is confirmed

                var updateResult = await userManager.UpdateAsync(adminUser);

                if (updateResult.Succeeded)
                {
                    // Ensure the user is in the Admin role
                    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
                else
                {
                    // Log errors during user update
                    foreach (var error in updateResult.Errors)
                    {
                        Console.WriteLine($"Error updating admin user: {error.Description}");
                    }
                }
            }
        }
    }
}