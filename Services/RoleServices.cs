using Microsoft.EntityFrameworkCore;
using ProgramPlatform.Data;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.Utilities;
using Exception = System.Exception;

namespace ProgramPlatform.Services;

public class RoleServices(ApplicationDbContext database, ILogger<RoleServices> logger) : IRoleInterface
{
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