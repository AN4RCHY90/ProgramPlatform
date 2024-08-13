using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramPlatform.Enums;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.ViewModels;

namespace ProgramPlatform.Controllers;

[Authorize]
public class UserController(ILogger<UserController> logger, IUserInterface userInterface,
    UserManager<ApplicationUserModel> userManager, IRoleInterface roleInterface,
    IAccountInterface accountInterface): Controller
{
    /// <summary>
    /// Retrieves a list of all users from the user interface and displays them in the Index view.
    /// </summary>
    /// <returns>An asynchronous task that returns an IActionResult.</returns>
    public async Task<IActionResult> Index()
    {
        var result = await userInterface.GetAllAsync();
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return View("Error", new ErrorViewModel { RequestId = result.ErrorMessage });
        }

        var model = result.Data.Select(u => new UserViewModel
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            EmailAddress = u.Email,
            MobileNumber = u.PhoneNumber,
            PreferredMode = u.PreferredMode,
            MultiFactor = u.MultiFactor,
            IsAdmin = u.IsAdmin,
            UserTypeList = u.UserTypesList,
            AccountId = u.AccountId,
            IsArchived = u.IsArchived
        }).ToList();

        return View(model);
    }

    /// <summary>
    /// Retrieves the details of a user with the specified ID and displays them in the Details view.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve details for.</param>
    /// <returns>An asynchronous task that returns an IActionResult.</returns>
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await userInterface.GetByIdAsync(id);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return NotFound();
        }
        
        var model = new UserViewModel
        {
            Id = result.Data.Id,
            FirstName = result.Data.FirstName,
            LastName = result.Data.LastName,
            EmailAddress = result.Data.Email,
            MobileNumber = result.Data.PhoneNumber,
            PreferredMode = result.Data.PreferredMode,
            MultiFactor = result.Data.MultiFactor,
            IsAdmin = result.Data.IsAdmin,
            UserTypeList = result.Data.UserTypesList,
            AccountId = result.Data.AccountId,
            IsArchived = result.Data.IsArchived
        };

        return View(result.Data);
    }

    /// <summary>
    /// Retrieves the current user's information, generates the necessary data, and returns the Create view
    /// for creating a new user.
    /// </summary>
    /// <returns>An asynchronous task that returns an IActionResult.</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var currentUserId = userManager.GetUserId(User);
        var currentUserResult = await userInterface.GetByIdAsync(Guid.Parse(currentUserId));
        if (!currentUserResult.Success)
        {
            logger.LogError(currentUserResult.ErrorMessage);
            return View("Error", new ErrorViewModel { RequestId = currentUserResult.ErrorMessage });
        }

        var currentUser = currentUserResult.Data;
        var isCommtelUser = currentUser.UserTypesList.Contains(UserTypeEnum.Commtel);

        if (isCommtelUser)
        {
            var rolesResult = await roleInterface.GetAllAsync();
            if (!rolesResult.Success)
            {
                logger.LogError(rolesResult.ErrorMessage);
                return View("Error", new ErrorViewModel { RequestId = rolesResult.ErrorMessage });
            }

            var accountsResult = await accountInterface.GetAllAsync();
            if (!accountsResult.Success)
            {
                logger.LogError(accountsResult.ErrorMessage);
                return View("Error", new ErrorViewModel { RequestId = accountsResult.ErrorMessage });
            }

            ViewBag.Roles = new SelectList(rolesResult.Data, "Id", "Name");
            ViewBag.Accounts = new SelectList(accountsResult.Data, "Id", "Name");
        }

        var model = new UserViewModel
        {
            AccountId = isCommtelUser ? Guid.Empty : currentUser.AccountId,
            UserTypeList = isCommtelUser ? new List<UserTypeEnum>() : currentUser.UserTypesList
        };
        
        return View(model);
    }

    /// <summary>
    /// Creates a new user based on the provided user view model.
    /// </summary>
    /// <param name="model">The user view model containing the user's information.</param>
    /// <returns>An asynchronous task that returns an IActionResult.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel model)
    {
        var currentUserId = userManager.GetUserId(User);
        var currentUserResult = await userInterface.GetByIdAsync(Guid.Parse(currentUserId));
        if (!currentUserResult.Success)
        {
            logger.LogError(currentUserResult.ErrorMessage);
            return View("Error", new ErrorViewModel { RequestId = currentUserResult.ErrorMessage });
        }

        var currentUser = currentUserResult.Data;
        var isCommtelUser = currentUser.UserTypesList.Contains(UserTypeEnum.Commtel);
        if (!isCommtelUser)
        {
            model.AccountId = currentUser.AccountId;
            model.UserTypeList = currentUser.UserTypesList;
        }
        
        if (!ModelState.IsValid)
        {
            if (isCommtelUser)
            {
                var rolesResult = await roleInterface.GetAllAsync();
                if (!rolesResult.Success)
                {
                    logger.LogError(rolesResult.ErrorMessage);
                    return View("Error", new ErrorViewModel { RequestId = rolesResult.ErrorMessage });
                }

                var accountsResult = await accountInterface.GetAllAsync();
                if (!accountsResult.Success)
                {
                    logger.LogError(accountsResult.ErrorMessage);
                    return View("Error", new ErrorViewModel { RequestId = accountsResult.ErrorMessage });
                }

                ViewBag.Roles = new SelectList(rolesResult.Data, "Id", "Name");
                ViewBag.Accounts = new SelectList(accountsResult.Data, "Id", "Name");
            }

            return View(model);
        }
        
        var user = new ApplicationUserModel
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.EmailAddress,
            PhoneNumber = model.MobileNumber,
            PreferredMode = model.PreferredMode,
            MultiFactor = model.MultiFactor,
            IsAdmin = model.IsAdmin,
            UserTypesList = model.UserTypeList,
            AccountId = model.AccountId,
            IsArchived = model.IsArchived
        };

        var result = await userInterface.CreateUserAsync(user);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);

            if (isCommtelUser)
            {
                var rolesResult = await roleInterface.GetAllAsync();
                if (rolesResult.Success)
                {
                    ViewBag.Roles = new SelectList(rolesResult.Data, "Id", "Name");
                }

                var accountsResult = await accountInterface.GetAllAsync();
                if (accountsResult.Success)
                {
                    ViewBag.Accounts = new SelectList(accountsResult.Data, "Id", "Name");
                }
            }

            return View("Error", new ErrorViewModel { RequestId = result.ErrorMessage });
        }

        return RedirectToAction("Index");
    }

    /// <summary>
    /// Retrieves a specific user by their ID from the user interface and displays them in the Edit view.
    /// </summary>
    /// <param name="id">The ID of the user to be retrieved.</param>
    /// <returns>An asynchronous task that returns an IActionResult.</returns>
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await userInterface.GetByIdAsync(id);
        if (!result.Success)
        {
            logger.LogWarning(result.ErrorMessage);
            return NotFound();
        }

        return View(result.Data);
    }

    /// <summary>
    /// Edits an ApplicationUserModel in the user interface.
    /// </summary>
    /// <param name="user">The ApplicationUserModel to be edited.</param>
    /// <returns>An asynchronous task that returns an IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationUserModel user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        var result = await userInterface.UpdateUserAsync(user);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return View(user);
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Retrieves a user from the user interface by their ID and displays them in the Delete view.
    /// </summary>
    /// <param name="id">The ID of the user to be deleted.</param>
    /// <returns>An asynchronous task that returns an IActionResult.</returns>
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await userInterface.GetByIdAsync(id);
        if (!result.Success)
        {
            logger.LogWarning(result.ErrorMessage);
            return NotFound();
        }

        return View(result.Data);
    }

    /// <summary>
    /// Deletes a user from the user interface and redirects to the Index view.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>An asynchronous task that returns an IActionResult.</returns>
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userResult = await userInterface.GetByIdAsync(id);
        if (!userResult.Success)
        {
            logger.LogError(userResult.ErrorMessage);
            return View();
        }

        var result = await userInterface.DeleteUserAsync(userResult.Data.Id);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return View();
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Archives a user by calling the ArchiveUserAsync method of the IUserInterface interface.
    /// </summary>
    /// <param name="id">The unique identifier of the user to archive.</param>
    /// <returns>An asynchronous task that returns an IActionResult.</returns>
    /// <remarks>
    /// This method calls the ArchiveUserAsync method of the IUserInterface interface with the specified id.
    /// If the operation is successful, the user is archived and the method redirects to the Index action.
    /// If the operation fails, an error message is logged and returned as a BadRequest response.
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Archive(Guid id)
    {
        var result = await userInterface.ArchiveUserAsync(id);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return BadRequest(result.ErrorMessage);
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Restores a user from the archive.
    /// </summary>
    /// <param name="id">The ID of the user to restore.</param>
    /// <returns>An asynchronous task that returns an IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await userInterface.RestoreUserAsync(id);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return BadRequest(result.ErrorMessage);
        }

        return RedirectToAction(nameof(Index));
    }
}