using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProgramPlatform.ViewModels;

namespace ProgramPlatform.Controllers;

public class AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
    ILogger<AuthenticationController> logger): Controller
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
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(String.Empty, "Invalid login attempt");
            logger.LogError("Invalid login attempt");
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