using Microsoft.AspNetCore.Identity;
using ProgramPlatform.Enums;
using ProgramPlatform.Models;

namespace ProgramPlatform.Data
{
    public static class SeedData
    {
        /// <summary>
        /// Initialises the seed data for the application.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to retrieve necessary service instances.</param>
        /// <param name="userManager">The user manager instance used for creating and managing users.</param>
        /// <exception cref="InvalidOperationException">Thrown when the admin email or password is not set in environment variables.</exception>
        /// <exception cref="Exception">Thrown when an error occurs during the initialisation process.</exception>
        public static async Task Initialise(IServiceProvider serviceProvider,
            UserManager<ApplicationUserModel> userManager)
        {
            var database = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRoleModel>>();

            var adminEmail = Environment.GetEnvironmentVariable("CirrusAdminEmail");
            var adminPassword = Environment.GetEnvironmentVariable("CirrusAdminPassword");
            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                throw new InvalidOperationException("Admin email or password is not set in environment variables.");
            }
            
            string[] roleNames = { "Commtel", "Installer", "ManagingAgent", "CommtelAdmin", "InstallerAdmin",
                "ManagingAgentAdmin" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new ApplicationRoleModel { Name = roleName });
                }
            }

            var commtelAccount = database.AccountModels.FirstOrDefault(a => a.Name == "Commtel");
            if (commtelAccount == null)
            {
                commtelAccount = new AccountModel
                {
                    Id = Guid.NewGuid(),
                    ReferenceNumber = Guid.NewGuid().ToString(),
                    Name = "Commtel",
                    AccountType = AccountTypeEnum.Commtel,
                    UserLimit = int.MaxValue,
                    FirstName = "Initial",
                    LastName = "Login",
                    Email = adminEmail,
                    Phone = "1234567890",
                    PreferredMode = PreferredModeEnum.Dark,
                    MultiFactor = false,
                    RoleManagement = true,
                    IsArchived = false
                };
                
                database.AccountModels.Add(commtelAccount);
                await database.SaveChangesAsync();
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUserModel
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Initial",
                    LastName = "Login",
                    AccountId = commtelAccount.Id,
                    IsAdmin = true
                };
                
                var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (createUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "CommtelAdmin");
                }
                else
                {
                    throw new Exception($"Failed to create default admin user: {string.Join(", ",
                        createUserResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}