using Microsoft.AspNetCore.Identity;
using ProgramPlatform.Models;
using ProgramPlatform.Services;

namespace ProgramPlatform.Data
{
    public static class SeedData
    {
        /// <summary>
        /// Initialises the application data.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve services.</param>
        /// <param name="userManager">The user manager used for user management.</param>
        /// <exception cref="InvalidOperationException">Thrown when the admin email or password is not set in environment variables.</exception>
        /// <exception cref="Exception">Thrown when there is an error while initialising the application data.</exception>
        public static async Task Initialise(IServiceProvider serviceProvider,
            UserManager<ApplicationUserModel> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRoleModel>>();
            var encryptionService = serviceProvider.GetRequiredService<EncryptionServices>();

            var adminEmail = Environment.GetEnvironmentVariable("CirrusAdminEmail");
            var adminPassword = Environment.GetEnvironmentVariable("CirrusAdminPassword");
            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                throw new InvalidOperationException("Admin email or password is not set in environment variables.");
            }

            // Combined roles including admin versions
            string[] roleNames = { "Commtel", "Installer", "ManagingAgent", "CommtelAdmin", "InstallerAdmin", "ManagingAgentAdmin" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new ApplicationRoleModel { Name = roleName });
                }
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUserModel
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                };
                var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (createUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "CommtelAdmin");
                }
                else
                {
                    throw new Exception($"Failed to create default admin user: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}