using ProgramPlatform.Models;
using ProgramPlatform.Utilities;

namespace ProgramPlatform.Interfaces;

public interface IRoleInterface
{
    public Task<ServiceResult<List<RoleModel>>> GetAllAsync();
    Task<ServiceResult<RoleModel>> GetByIdAsync(Guid id);
    Task<ServiceResult<RoleModel>> CreateRoleAsync(RoleModel role);
    Task<ServiceResult<RoleModel>> UpdateRoleAsync(RoleModel role);
    Task<ServiceResult<bool>> DeleteRoleAsync(Guid id);
    Task<ServiceResult<bool>> ArchiveRoleAsync(Guid id);
    Task<ServiceResult<bool>> RestoreRoleAsync(Guid id);
}