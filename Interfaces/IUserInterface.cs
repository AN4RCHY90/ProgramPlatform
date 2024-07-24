using ProgramPlatform.Models;
using ProgramPlatform.Utilities;

namespace ProgramPlatform.Interfaces;

public interface IUserInterface
{
    Task<ServiceResult<UserModel>> CreateUserAsync(UserModel user);
}