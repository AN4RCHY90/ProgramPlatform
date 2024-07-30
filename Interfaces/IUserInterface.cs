using ProgramPlatform.Models;
using ProgramPlatform.Utilities;

namespace ProgramPlatform.Interfaces;

public interface IUserInterface
{
    public Task<ServiceResult<List<UserModel>>> GetAllAsync();
    public Task<ServiceResult<UserModel>> GetByIdAsync(Guid id);
    public Task<ServiceResult<UserModel>> CreateUserAsync(UserModel user);
    public Task<ServiceResult<UserModel>> UpdateUserAsync(UserModel user);
    public Task<ServiceResult<bool>> DeleteUserAsync(Guid id);
    public Task<ServiceResult<bool>> ArchiveUserAsync(Guid id);
    public Task<ServiceResult<bool>> RestoreUserAsync(Guid id);
}