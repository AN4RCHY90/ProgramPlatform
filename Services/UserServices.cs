using Microsoft.EntityFrameworkCore;
using ProgramPlatform.Data;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.Utilities;

namespace ProgramPlatform.Services;

public class UserServices(ApplicationDbContext database, ILogger<UserServices> logger): IUserInterface
{
    /// <summary>
    /// Retrieves all users asynchronously.
    /// </summary>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// The Task's result contains a ServiceResult object with the list of retrieved users if successful,
    /// or an error message if unsuccessful.
    /// </returns>
    public async Task<ServiceResult<List<UserModel>>> GetAllAsync()
    {
        try
        {
            var users = await database.UserModels.ToListAsync();
            return ServiceResult<List<UserModel>>.Successful(users);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error fetching all users");
            return ServiceResult<List<UserModel>>.Failure("Error fetching all users");
        }
    }

    /// <summary>
    /// Retrieves a user by their ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task's result contains a
    /// ServiceResult object with the retrieved user if successful, or an error message if unsuccessful.</returns>
    public async Task<ServiceResult<UserModel>> GetByIdAsync(Guid id)
    {
        try
        {
            var user = await database.UserModels.FindAsync(id);
            if (user == null)
            {
                logger.LogInformation("User not found");
                return ServiceResult<UserModel>.Failure("User not found");
            }

            return ServiceResult<UserModel>.Successful(user);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error fetching user with ID {Id}", id);
            return ServiceResult<UserModel>.Failure("Error fetching user");
        }
    }

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The UserModel object containing the user details.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task's result contains a
    /// ServiceResult object with the created user if successful, or an error message if unsuccessful.</returns>
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

    /// <summary>
    /// Updates a user asynchronously.
    /// </summary>
    /// <param name="user">The UserModel object containing the updated user data.</param>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// The Task's result contains a ServiceResult object with the updated user if successful,
    /// or an error message if unsuccessful.
    /// </returns>
    public async Task<ServiceResult<UserModel>> UpdateUserAsync(UserModel user)
    {
        try
        {
            database.UserModels.Update(user);
            await database.SaveChangesAsync();
            return ServiceResult<UserModel>.Successful(user);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating user with ID: {Id}", user.Id);
            return ServiceResult<UserModel>.Failure("Error updating user");
        }
    }

    /// <summary>
    /// Deletes a user asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// The Task's result contains a ServiceResult object with a boolean value indicating
    /// whether the deletion was successful or not, and an error message if unsuccessful.
    /// </returns>
    public async Task<ServiceResult<bool>> DeleteUserAsync(Guid id)
    {
        try
        {
            var user = await database.UserModels.FindAsync(id);
            if (user == null)
            {
                logger.LogInformation("User with ID: {Id}, not found", id);
                return ServiceResult<bool>.Failure("Role not found");
            }

            database.UserModels.Remove(user);
            await database.SaveChangesAsync();
            return ServiceResult<bool>.Successful(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting user with ID: {Id}", id);
            return ServiceResult<bool>.Failure("Error deleting user");
        }
    }

    /// <summary>
    /// Archives a user asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to be archived.</param>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// The Task's result contains a ServiceResult object with a boolean value indicating
    /// whether the user was successfully archived or not, and an error message if unsuccessful.
    /// </returns>
    public async Task<ServiceResult<bool>> ArchiveUserAsync(Guid id)
    {
        try
        {
            var user = await database.UserModels.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                logger.LogError("User not found, can not archive user with ID: {Id}", id);
                return ServiceResult<bool>.Failure("User not found, can not archive them");
            }
            
            user.IsArchived = false;
            database.UserModels.Update(user);
            await database.SaveChangesAsync();
            return ServiceResult<bool>.Successful(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error archiving user with ID {Id}", id);
            return ServiceResult<bool>.Failure("Error archiving user");
        }
    }

    /// <summary>
    /// Restores a user asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to be restored.</param>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// The Task's result contains a ServiceResult object with a boolean value indicating whether the user
    /// was restored successfully, or an error message if unsuccessful.
    /// </returns>
    public async Task<ServiceResult<bool>> RestoreUserAsync(Guid id)
    {
        try
        {
            var user = await database.UserModels.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                logger.LogError("User not found, can not restore user with ID: {Id}", id);
                return ServiceResult<bool>.Failure("User not found, can not restore user");
            }
            
            user.IsArchived = false;
            database.UserModels.Update(user);
            await database.SaveChangesAsync();
            return ServiceResult<bool>.Successful(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error restoring user with ID: {Id}", id);
            return ServiceResult<bool>.Failure("Error restoring user");
        }
    }
}