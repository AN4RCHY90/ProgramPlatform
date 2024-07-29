using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProgramPlatform.Models;
using ProgramPlatform.ViewModels;

namespace ProgramPlatform.Controllers;

public class AuthenticationController(
    SignInManager<ApplicationUserModel> signInManager,
    UserManager<ApplicationUserModel> userManager,
    ILogger<AuthenticationController> logger)
    : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                logger.LogWarning("User account not found: {ModelEmail}", model.Email);
            }
            
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, 
                model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                logger.LogInformation("Login successful");
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            logger.LogError("Invalid login attempt");
        }
        else
        {
            logger.LogWarning("Model state invalid, AuthC");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}