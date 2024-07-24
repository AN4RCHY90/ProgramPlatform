using ProgramPlatform.Models;
using ProgramPlatform.Utilities;

namespace ProgramPlatform.Interfaces;

public interface IAccountInterface
{
    Task<ServiceResult<List<AccountModel>>> GetAllAsync();
    Task<ServiceResult<AccountModel>> GetByReferenceNumberAsync(string referenceNumber);
    Task<ServiceResult<AccountModel>> CreateAccountAsync(AccountModel account);
    Task<ServiceResult<AccountModel>> UpdateAccountAsync(AccountModel account);
    Task<ServiceResult<bool>> DeleteAccountAsync(Guid id);
    Task<ServiceResult<bool>> ArchiveAccountAsync(string referenceNumber);
    Task<ServiceResult<bool>> RestoreAccountAsync(string referenceNumber);
}