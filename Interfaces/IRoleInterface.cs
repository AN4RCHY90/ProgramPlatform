using ProgramPlatform.Models;
using ProgramPlatform.Utilities;

namespace ProgramPlatform.Interfaces;

public interface IRoleInterface
{
    public Task<ServiceResult<List<ApplicationRoleModel>>> GetAllAsync();
    Task<ServiceResult<ApplicationRoleModel>> GetByIdAsync(Guid id);
    Task<ServiceResult<ApplicationRoleModel>> CreateRoleAsync(ApplicationRoleModel role);
    Task<ServiceResult<ApplicationRoleModel>> UpdateRoleAsync(ApplicationRoleModel role);
    Task<ServiceResult<bool>> DeleteRoleAsync(Guid id);
    Task<ServiceResult<bool>> ArchiveRoleAsync(Guid id);
    Task<ServiceResult<bool>> RestoreRoleAsync(Guid id);
}