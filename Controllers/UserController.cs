using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.ViewModels;

namespace ProgramPlatform.Controllers;

[Authorize]
public class UserController(ILogger<UserController> logger, IUserInterface userInterface): Controller
{
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
            MobileNumber = u.MobileNumber,
            PreferredMode = u.PreferredMode,
            MultiFactor = u.MultiFactor,
            IsAdmin = u.IsAdmin,
            UserTypeList = u.UserTypesList,
            AccountId = u.AccountId,
            IsArchived = u.IsArchived
        }).ToList();

        return View(model);
    }

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
            MobileNumber = result.Data.MobileNumber,
            PreferredMode = result.Data.PreferredMode,
            MultiFactor = result.Data.MultiFactor,
            IsAdmin = result.Data.IsAdmin,
            UserTypeList = result.Data.UserTypesList,
            AccountId = result.Data.AccountId,
            IsArchived = result.Data.IsArchived
        };

        return View(model);
    }

    
}