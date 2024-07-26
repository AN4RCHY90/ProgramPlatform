using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.ViewModels;

namespace ProgramPlatform.Controllers;

[Authorize]
public class RoleController(ILogger<RoleController> logger, IRoleInterface roleInterface): Controller
{
    /// <summary>
    /// Retrieves all role models and maps them to role view models.
    /// </summary>
    /// <returns>
    /// The action result representing the view with the list of role view models.
    /// </returns>
    public async Task<IActionResult> Index()
    {
        var result = await roleInterface.GetAllAsync();
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return View("Error", new ErrorViewModel { RequestId = result.ErrorMessage });
        }

        var model = result.Data.Select(r => new RoleViewModel
        {
            Id = r.Id,
            RoleName = r.RoleName,
            UserType = r.UserType,
            PullDeviceProgramming = r.PullDeviceProgramming,
            ApiAccess = r.ApiAccess,
            DashboardAccess = r.DashboardAccess,
            ProgrammingRollBack = r.ProgrammingRollBack,
            ActivityLogPermissions = r.ActivityLogPermissions,
            CallPointPermissions = r.CallPointPermissions,
            IncomingCallPermissions = r.IncomingCallPermissions,
            TimePeriodPermissions = r.TimePeriodPermissions,
            RelayOpsPermissions = r.RelayOpsPermissions,
            SmsCommandsPermissions = r.SmsCommandsPermissions,
            AlertPermissions = r.AlertPermissions,
            PanelSettingsPermissions = r.PanelSettingsPermissions,
            IsArchived = r.IsArchived
        }).ToList();
        
        return View(model);
    }

    /// <summary>
    /// Retrieves a role with the specified ID and maps it to a role view model.
    /// </summary>
    /// <param name="id">The ID of the role to retrieve.</param>
    /// <returns>The action result representing the view with the role view model.</returns>
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await roleInterface.GetByIdAsync(id);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return NotFound();
        }

        var model = new RoleViewModel
        {
            Id = result.Data.Id,
            RoleName = result.Data.RoleName,
            UserType = result.Data.UserType,
            PullDeviceProgramming = result.Data.PullDeviceProgramming,
            ApiAccess = result.Data.ApiAccess,
            DashboardAccess = result.Data.DashboardAccess,
            ProgrammingRollBack = result.Data.ProgrammingRollBack,
            ActivityLogPermissions = result.Data.ActivityLogPermissions,
            CallPointPermissions = result.Data.CallPointPermissions,
            IncomingCallPermissions = result.Data.IncomingCallPermissions,
            TimePeriodPermissions = result.Data.TimePeriodPermissions,
            RelayOpsPermissions = result.Data.RelayOpsPermissions,
            SmsCommandsPermissions = result.Data.SmsCommandsPermissions,
            AlertPermissions = result.Data.AlertPermissions,
            PanelSettingsPermissions = result.Data.PanelSettingsPermissions,
            IsArchived = result.Data.IsArchived
        };
        
        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    /// <summary>
    /// Creates a new role using the provided role view model.
    /// </summary>
    /// <param name="model">The role view model containing the role details.</param>
    /// <returns>The action result representing the view for the created role.</returns
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var role = new RoleModel
            {
                Id = Guid.NewGuid(),
                RoleName = model.RoleName,
                UserType = model.UserType,
                ApiAccess = model.ApiAccess,
                DashboardAccess = model.DashboardAccess,
                ProgrammingRollBack = model.ProgrammingRollBack,
                ActivityLogPermissions = model.ActivityLogPermissions,
                CallPointPermissions = model.CallPointPermissions,
                IncomingCallPermissions = model.IncomingCallPermissions,
                TimePeriodPermissions = model.TimePeriodPermissions,
                RelayOpsPermissions = model.RelayOpsPermissions,
                SmsCommandsPermissions = model.SmsCommandsPermissions,
                AlertPermissions = model.AlertPermissions,
                PanelSettingsPermissions = model.PanelSettingsPermissions,
                IsArchived = model.IsArchived
            };

            var result = await roleInterface.CreateRoleAsync(role);
            if (result.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            
            ModelState.AddModelError(String.Empty, result.ErrorMessage);
        }

        return View(model);
    }

    /// <summary>
    /// Retrieves the role model with the specified ID and maps it to a role view model for editing.
    /// </summary>
    /// <param name="id">The ID of the role model to be edited.</param>
    /// <returns>The action result representing the view with the editable role view model.</returns>
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await roleInterface.GetByIdAsync(id);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return NotFound();
        }

        var model = new RoleViewModel
        {
            Id = result.Data.Id,
            RoleName = result.Data.RoleName,
            UserType = result.Data.UserType,
            ApiAccess = result.Data.ApiAccess,
            DashboardAccess = result.Data.DashboardAccess,
            ProgrammingRollBack = result.Data.ProgrammingRollBack,
            ActivityLogPermissions = result.Data.ActivityLogPermissions,
            CallPointPermissions = result.Data.CallPointPermissions,
            IncomingCallPermissions = result.Data.IncomingCallPermissions,
            TimePeriodPermissions = result.Data.TimePeriodPermissions,
            RelayOpsPermissions = result.Data.RelayOpsPermissions,
            SmsCommandsPermissions = result.Data.SmsCommandsPermissions,
            AlertPermissions = result.Data.AlertPermissions,
            PanelSettingsPermissions = result.Data.PanelSettingsPermissions,
            IsArchived = result.Data.IsArchived
        };
        
        return View(model);
    }

    /// <summary>
    /// Updates the specified role with the provided data.
    /// </summary>
    /// <param name="model">The updated role view model.</param>
    /// <returns>The action result representing the view with the updated role view model.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(RoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await roleInterface.GetByIdAsync(model.Id);
            if (!result.Success)
            {
                logger.LogError(result.ErrorMessage);
                return NotFound();
            }

            var role = result.Data;
            role.RoleName = model.RoleName;
            role.UserType = model.UserType;
            role.ApiAccess = model.ApiAccess;
            role.DashboardAccess = model.DashboardAccess;
            role.ProgrammingRollBack = model.ProgrammingRollBack;
            role.ActivityLogPermissions = model.ActivityLogPermissions;
            role.CallPointPermissions = model.CallPointPermissions;
            role.IncomingCallPermissions = model.IncomingCallPermissions;
            role.TimePeriodPermissions = model.TimePeriodPermissions;
            role.RelayOpsPermissions = model.RelayOpsPermissions;
            role.SmsCommandsPermissions = model.SmsCommandsPermissions;
            role.AlertPermissions = model.AlertPermissions;
            role.PanelSettingsPermissions = model.PanelSettingsPermissions;
            role.IsArchived = model.IsArchived;

            var updateResult = await roleInterface.UpdateRoleAsync(role);
            if (updateResult.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            
            ModelState.AddModelError(String.Empty, updateResult.ErrorMessage);
        }
        return View(model);
    }

    /// <summary>
    /// Deletes a role by its ID.
    /// </summary>
    /// <param name="id">The ID of the role to be deleted.</param>
    /// <returns>An action result representing the view after the role has been deleted.</returns>
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await roleInterface.GetByIdAsync(id);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
            return NotFound();
        }

        var model = new RoleViewModel
        {
            Id = result.Data.Id,
            RoleName = result.Data.RoleName,
            UserType = result.Data.UserType,
            ApiAccess = result.Data.ApiAccess,
            DashboardAccess = result.Data.DashboardAccess,
            ProgrammingRollBack = result.Data.ProgrammingRollBack,
            ActivityLogPermissions = result.Data.ActivityLogPermissions,
            CallPointPermissions = result.Data.CallPointPermissions,
            IncomingCallPermissions = result.Data.IncomingCallPermissions,
            TimePeriodPermissions = result.Data.TimePeriodPermissions,
            RelayOpsPermissions = result.Data.RelayOpsPermissions,
            SmsCommandsPermissions = result.Data.SmsCommandsPermissions,
            AlertPermissions = result.Data.AlertPermissions,
            PanelSettingsPermissions = result.Data.PanelSettingsPermissions,
            IsArchived = result.Data.IsArchived
        };

        return View(model);
    }

    /// <summary>
    /// Deletes a role with the specified ID and redirects to the index action.
    /// </summary>
    /// <param name="id">The ID of the role to delete.</param>
    /// <returns>
    /// The action result representing the redirect to the index action.
    /// </returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var result = await roleInterface.DeleteRoleAsync(id);
        if (!result.Success)
        {
            logger.LogError(result.ErrorMessage);
        }

        return RedirectToAction(nameof(Index));
    }
}