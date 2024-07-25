using Microsoft.EntityFrameworkCore;
using ProgramPlatform.Data;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.Utilities;
using Exception = System.Exception;

namespace ProgramPlatform.Services;

public class RoleServices(ApplicationDbContext database, ILogger<RoleServices> logger) : IRoleInterface
{
    /// <summary>
    /// Retrieves all roles asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains a <see cref="ServiceResult{T}"/> where T is <see cref="List{RoleModel}"/>.
    /// The service result indicates whether the operation was successful, and if so, contains the list of roles.</returns>
    public async Task<ServiceResult<List<RoleModel>>> GetAllAsync()
    {
        try
        {
            var roles = await database.RoleModels.ToListAsync();
            return ServiceResult<List<RoleModel>>.Successful(roles);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error fetching all roles");
            return ServiceResult<List<RoleModel>>.Failure("Error fetching all roles");
        }
    }

    /// <summary>
    /// Retrieves a role by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the role to retrieve.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains a <see cref="ServiceResult{T}"/> where T is <see cref="RoleModel"/>.
    /// The service result indicates whether the operation was successful, and if so, contains the role.</returns>
    public async Task<ServiceResult<RoleModel>> GetByIdAsync(Guid id)
    {
        try
        {
            var role = await database.RoleModels.FindAsync(id);
            if (role == null)
            {
                logger.LogInformation("Role not found");
                return ServiceResult<RoleModel>.Failure("Role not found");
            }

            return ServiceResult<RoleModel>.Successful(role);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error fetching role with ID {Id}", id);
            return ServiceResult<RoleModel>.Failure("Error fetching role");
        }
    }

    /// <summary>
    /// Creates a new role asynchronously.
    /// </summary>
    /// <param name="role">The role model to create.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains a <see cref="ServiceResult{T}"/> where T is <see cref="RoleModel"/>.
    /// The service result indicates whether the operation was successful, and if so, contains the created role.</returns>
    public async Task<ServiceResult<RoleModel>> CreateRoleAsync(RoleModel role)
    {
        try
        {
            database.RoleModels.Add(role);
            await database.SaveChangesAsync();
            return ServiceResult<RoleModel>.Successful(role);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating role");
            return ServiceResult<RoleModel>.Failure("Error creating role");
        }
    }

    /// <summary>
    /// Updates the specified role asynchronously.
    /// </summary>
    /// <param name="role">The role to be updated.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains a <see cref="ServiceResult{T}"/> where T is <see cref="RoleModel"/>.
    /// The service result indicates whether the operation was successful, and if so, contains the updated role.</returns>
    public async Task<ServiceResult<RoleModel>> UpdateRoleAsync(RoleModel role)
    {
        try
        {
            database.RoleModels.Update(role);
            await database.SaveChangesAsync();
            return ServiceResult<RoleModel>.Successful(role);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating role with ID {Id}", role.Id);
            return ServiceResult<RoleModel>.Failure("Error updating role");
        }
    }

    /// <summary>
    /// Deletes a role asynchronously.
    /// </summary>
    /// <param name="id">The ID of the role to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.
    /// The task result contains a <see cref="ServiceResult{T}"/> where T is <see cref="bool"/>.
    /// The service result indicates whether the operation was successful.</returns>
    public async Task<ServiceResult<bool>> DeleteRoleAsync(Guid id)
    {
        try
        {
            var role = await database.RoleModels.FindAsync(id);
            if (role == null)
            {
                logger.LogInformation("Role with ID {Id}, not found", id);
                return ServiceResult<bool>.Failure("Role not found");
            }

            database.RoleModels.Remove(role);
            await database.SaveChangesAsync();
            return ServiceResult<bool>.Successful(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting role");
            return ServiceResult<bool>.Failure("Error deleting role");
        }
    }

    public async Task<ServiceResult<bool>> ArchiveRoleAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResult<bool>> RestoreRoleAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}