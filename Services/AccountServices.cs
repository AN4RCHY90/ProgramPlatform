using Microsoft.EntityFrameworkCore;
using ProgramPlatform.Data;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.Utilities;

namespace ProgramPlatform.Services;

public class AccountServices(ApplicationDbContext database, ILogger<AccountServices> logger): IAccountInterface
{
    /// <summary>
    /// Retrieves a list of all accounts asynchronously.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains
    /// a ServiceResult object with a list of AccountModel objects if successful, or an error
    /// message if failed.</returns>
    public async Task<ServiceResult<List<AccountModel>>> GetAllAsync()
    {
        try
        {
            var accountList = await database.AccountModels.ToListAsync();

            return ServiceResult<List<AccountModel>>.Successful(accountList);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error pulling accounts list");
            return ServiceResult<List<AccountModel>>.Failure("Failed to pull accounts list");
        }
    }

    /// <summary>
    /// Retrieves an account by its reference number asynchronously.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the account.</param>
    /// <returns>A <see cref="Task{T}"/> that represents the asynchronous operation. The task result contains
    /// a <see cref="ServiceResult{T}"/> object with an <see cref="AccountModel"/> if the account is found,
    /// or an error message if the account is not found or an error occurred.</returns>
    public async Task<ServiceResult<AccountModel>> GetByReferenceNumberAsync(string referenceNumber)
    {
        try
        {
            var account = await database.AccountModels.FirstOrDefaultAsync(a => a.ReferenceNumber == referenceNumber);
            if (account == null)
            {
                return ServiceResult<AccountModel>.Failure("Account not found");
            }

            return ServiceResult<AccountModel>.Successful(account);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error finding account with reference number {ReferenceNumber}", referenceNumber);
            return ServiceResult<AccountModel>.Failure($"Error finding account with reference number {referenceNumber}");
        }
    }

    /// <summary>
    /// Creates a new account asynchronously.
    /// </summary>
    /// <param name="account">The AccountModel object containing the details of the account to be created.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains
    /// a ServiceResult object with the created AccountModel object if successful, or an error
    /// message if failed.</returns>
    public async Task<ServiceResult<AccountModel>> CreateAccountAsync(AccountModel account)
    {
        try
        {
            database.AccountModels.Add(account);
            await database.SaveChangesAsync();
            return ServiceResult<AccountModel>.Successful(account);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving new account details");
            return ServiceResult<AccountModel>.Failure("Error saving new account details");
        }
    }

    /// <summary>
    /// Updates an account asynchronously.
    /// </summary>
    /// <param name="account">The account to be updated.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains
    /// a ServiceResult object with the updated AccountModel object if the update is successful,
    /// or an error message if the update fails.</returns>
    public async Task<ServiceResult<AccountModel>> UpdateAccountAsync(AccountModel account)
    {
        try
        {
            database.AccountModels.Update(account);
            await database.SaveChangesAsync();
            return ServiceResult<AccountModel>.Successful(account);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating account details");
            return ServiceResult<AccountModel>.Failure("Error updated account details");
        }
    }

    /// <summary>
    /// Deletes an account asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the account to delete.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains
    /// a ServiceResult object with a boolean indicating whether the deletion was successful
    /// or not, and an error message if the deletion failed.</returns>
    public async Task<ServiceResult<bool>> DeleteAccountAsync(Guid id)
    {
        try
        {
            var account = await database.AccountModels.FindAsync(id);
            if (account == null)
            {
                return ServiceResult<bool>.Failure("Account not found");
            }

            database.AccountModels.Remove(account);
            await database.SaveChangesAsync();
            return ServiceResult<bool>.Successful(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting account");
            return ServiceResult<bool>.Failure("Error deleting account");
        }
    }

    /// <summary>
    /// Archives an account asynchronously based on the provided reference number.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the account to be archived.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains
    /// a ServiceResult object with a bool indicating the success of the operation. If successful,
    /// the bool will be set to true. Otherwise, an error message will be provided.</returns>
    public async Task<ServiceResult<bool>> ArchiveAccountAsync(string referenceNumber)
    {
        try
        {
            var account = await database.AccountModels.FirstOrDefaultAsync(a => a.ReferenceNumber == referenceNumber);
            if (account == null)
            {
                return ServiceResult<bool>.Failure("Account not found");
            }

            account.IsArchived = true;
            database.AccountModels.Update(account);
            await database.SaveChangesAsync();
            return ServiceResult<bool>.Successful(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error archiving account");
            return ServiceResult<bool>.Failure("Error archiving account");
        }
    }

    /// <summary>
    /// Restores an archived account asynchronously.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the account to restore.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains
    /// a ServiceResult object with a boolean value indicating the success or failure of the operation,
    /// along with an error message if failed.</returns>
    public async Task<ServiceResult<bool>> RestoreAccountAsync(string referenceNumber)
    {
        try
        {
            var account = await database.AccountModels.FirstOrDefaultAsync(a => a.ReferenceNumber == referenceNumber);
            if (account == null)
            {
                return ServiceResult<bool>.Failure("Account not found");
            }

            account.IsArchived = false;
            database.AccountModels.Update(account);
            await database.SaveChangesAsync();
            return ServiceResult<bool>.Successful(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error restoring account");
            return ServiceResult<bool>.Failure("Error restoring account");
        }
    }
}