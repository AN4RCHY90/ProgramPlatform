using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramPlatform.Enums;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.Utilities;
using ProgramPlatform.ViewModels;

namespace ProgramPlatform.Controllers;

[Authorize]
public class AccountController(IAccountInterface accountInterface, IZohoInterface zohoInterface,
    IUserInterface userInterface, ILogger<AccountController> logger): Controller
{
    /// <summary>
    /// Retrieves a filtered list of accounts based on the provided criteria.
    /// </summary>
    /// <param name="referenceNumber">The reference number to filter accounts by. Can be null or empty.</param>
    /// <param name="name">The name to filter accounts by. Can be null or empty.</param>
    /// <param name="accountType">The account type to filter accounts by. Can be null.</param>
    /// <param name="status">The status to filter accounts by. Can be null or empty.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains the filtered list of accounts.</returns>
    public async Task<IActionResult> Index(string referenceNumber, string name,
        AccountTypeEnum? accountType, string status)
    {
        ModelState.Remove("referenceNumber");
        ModelState.Remove("name");
        ModelState.Remove("status");
        
        var result = await accountInterface.GetAllAsync();

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(new List<AccountModel>());
        }

        var accounts = result.Data;

        if (!string.IsNullOrEmpty(referenceNumber))
        {
            accounts = accounts.Where(a => a.ReferenceNumber.ToString().Contains(referenceNumber)).ToList();
        }

        if (!string.IsNullOrEmpty(name))
        {
            accounts = accounts.Where(a => a.Name
                .Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (accountType.HasValue)
        {
            accounts = accounts.Where(a => a.AccountType == accountType.Value).ToList();
        }

        if (!string.IsNullOrEmpty(status))
        {
            if (status == "active")
            {
                accounts = accounts.Where(a => !a.IsArchived).ToList();
            }
            else if (status == "archived")
            {
                accounts = accounts.Where(a => a.IsArchived).ToList();
            }
        }

        ViewData["referenceNumber"] = referenceNumber;
        ViewData["name"] = name;
        ViewData["accountType"] = accountType;
        ViewData["status"] = status;
        
        return View(accounts);
    }


    /// <summary>
    /// Retrieves the details of an account based on the provided reference number.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the account.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains the details of the account.</returns>
    public async Task<IActionResult> Details(string referenceNumber)
    {
        var result = await accountInterface.GetByReferenceNumberAsync(referenceNumber);
        if (!result.Success)
        {
            logger.LogWarning(result.ErrorMessage);
            return NotFound();
        }

        return View(result.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateAccountViewModel());
    }

    /// <summary>
    /// Retrieves data from Zoho based on a reference number and populates a CreateAccountViewModel with the retrieved data.
    /// </summary>
    /// <param name="referenceNumber">The reference number to search for in Zoho.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains a populated
    /// CreateAccountViewModel if the data was successfully fetched from Zoho, otherwise returns a view with
    /// an error message.</returns>
    [HttpPost]
    public async Task<IActionResult> FetchZohoData(string referenceNumber)
    {
        var existingAccount = await accountInterface.GetByReferenceNumberAsync(referenceNumber);
        if (existingAccount.Success)
        {
            ModelState.AddModelError(String.Empty, "Ac account with this reference number already exists");
            return View("Create", new CreateAccountViewModel { ReferenceNumber = referenceNumber });
        }

        try
        {
            var zohoAccount = await zohoInterface.GetAccountByReferenceNumberAsync(referenceNumber);
            var viewModel = new CreateAccountViewModel
            {
                ReferenceNumber = referenceNumber,
                AccountName = zohoAccount.Name,
                EmailAddress = zohoAccount.Email,
                PhoneNumber = zohoAccount.Phone,
                SubscriptionExpiryDate = DateTime.Parse(zohoAccount.Sub_Exp),
                AccountType = zohoAccount.Account_Type
            };
            return View("Create", viewModel);
        }
        catch (Exception e)
        {
            ModelState.AddModelError(String.Empty, "An error occurred while fetching the data from Zoho");
            logger.LogError(e, "Error fetching data from Zoho for reference number {ReferenceNumber}", referenceNumber);
            return View("Create", new CreateAccountViewModel { ReferenceNumber = referenceNumber });
        }
    }

    /// <summary>
    /// Creates a new account based on the provided account information.
    /// </summary>
    /// <param name="model">The information of the account to be created.</param>
    /// <returns>A Task representing the asynchronous operation. The Task result contains the created account.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAccountViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var accountTypeEnum = UserTypeMapper.ParseAccountType(model.AccountType);
        var account = new AccountModel
        {
            ReferenceNumber = model.ReferenceNumber,
            Name = model.AccountName,
            AccountType = accountTypeEnum,
            SubscriptionExpiry = model.SubscriptionExpiryDate,
            FirstName = model.AdminFirstName,
            LastName = model.AdminLastName,
            Email = model.EmailAddress,
            Phone = model.PhoneNumber
        };

        var result = await accountInterface.CreateAccountAsync(account);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return View(model);
        }

        var adminUser = new UserModel
        {
            FirstName = model.AdminFirstName,
            LastName = model.AdminLastName,
            EmailAddress = model.EmailAddress,
            AccountId = account.Id,
            IsAdmin = true,
            UserTypesList = new List<UserTypeEnum> { UserTypeEnum.Admin, UserTypeMapper.MapAccountTypeToUserType(accountTypeEnum) }
        };

        var userResult = await userInterface.CreateUserAsync(adminUser);
        if (!userResult.Success)
        {
            logger.LogError(userResult.ErrorMessage);
            ModelState.AddModelError(string.Empty, "Error creating admin user. Please try again.");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Retrieves an account by its reference number and renders the edit view for that account.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the account to edit.</param>
    /// <returns>An asynchronous task that represents the operation. The task result contains an
    /// IActionResult representing the edit view.</returns>
    public async Task<IActionResult> Edit(string referenceNumber)
    {
        var result = await accountInterface.GetByReferenceNumberAsync(referenceNumber);
        if (!result.Success)
        {
            logger.LogWarning(result.ErrorMessage);
            return NotFound();
        }

        return View(result.Data);
    }

    /// <summary>
    /// Updates the specified account with the provided information.
    /// </summary>
    /// <param name="account">The updated account information.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains the
    /// updated account information.</returns>
    [HttpPost]
    public async Task<IActionResult> Edit(AccountModel account)
    {
        if (!ModelState.IsValid)
        {
            return View(account);
        }

        var result = await accountInterface.UpdateAccountAsync(account);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return View(account);
        }
        
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Retrieves an account based on the provided reference number for deletion.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the account to be deleted.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains an
    /// IActionResult representing the deletion of the account.</returns>
    public async Task<IActionResult> Delete(string referenceNumber)
    {
        var result = await accountInterface.GetByReferenceNumberAsync(referenceNumber);
        if (!result.Success)
        {
            logger.LogWarning(result.ErrorMessage);
            return NotFound();
        }

        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string referenceNumber)
    {
        var accountResult = await accountInterface.GetByReferenceNumberAsync(referenceNumber);
        if (!accountResult.Success)
        {
            logger.LogError(accountResult.ErrorMessage);
            return View();
        }

        var result = await accountInterface.DeleteAccountAsync(accountResult.Data.Id);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return View();
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Archives an account by setting its status to archived.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the account to archive.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result indicates
    /// whether the account was successfully archived.</returns>
    [HttpPost]
    public async Task<IActionResult> Archive(string referenceNumber)
    {
        var result = await accountInterface.ArchiveAccountAsync(referenceNumber);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return BadRequest(result.ErrorMessage);
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Restores an account using the provided reference number.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the account to restore.</param>
    /// <returns>A Task that represents the asynchronous operation. The Task result contains a
    /// ServiceResult indicating whether the account was successfully restored or not.</returns>
    [HttpPost]
    public async Task<IActionResult> Restore(string referenceNumber)
    {
        var result = await accountInterface.RestoreAccountAsync(referenceNumber);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return BadRequest(result.ErrorMessage);
        }

        return RedirectToAction(nameof(Index));
    }
}