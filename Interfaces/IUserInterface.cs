using ProgramPlatform.Models;
using ProgramPlatform.Utilities;

namespace ProgramPlatform.Interfaces;

public interface IUserInterface
{
    public Task<ServiceResult<List<ApplicationUserModel>>> GetAllAsync();
    public Task<ServiceResult<ApplicationUserModel>> GetByIdAsync(Guid id);
    public Task<ServiceResult<ApplicationUserModel>> CreateUserAsync(ApplicationUserModel user);
    public Task<ServiceResult<ApplicationUserModel>> UpdateUserAsync(ApplicationUserModel user);
    public Task<ServiceResult<bool>> DeleteUserAsync(Guid id);
    public Task<ServiceResult<bool>> ArchiveUserAsync(Guid id);
    public Task<ServiceResult<bool>> RestoreUserAsync(Guid id);
}