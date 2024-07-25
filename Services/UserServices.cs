using ProgramPlatform.Data;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.Utilities;

namespace ProgramPlatform.Services;

public class UserServices(ApplicationDbContext database, ILogger<UserServices> logger): IUserInterface
{
    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The UserModel object containing the user details.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task's result contains a ServiceResult object with the created user if successful, or an error message if unsuccessful.</returns>
    public async Task<ServiceResult<UserModel>> CreateUserAsync(UserModel user)
    {
        try
        {
            database.UserModels.Add(user);
            await database.SaveChangesAsync();
            logger.LogInformation("User {UserFirstName} {UserLastName} successfully added",
                user.FirstName, user.LastName);
            return ServiceResult<UserModel>.Successful(user);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating user {firstName} {lastName}", user.FirstName, user.LastName);
            return ServiceResult<UserModel>.Failure("Error creating user");
        }
    }
}