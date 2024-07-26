using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProgramPlatform.Controllers;

[Authorize]
public class UserController(ILogger<UserController> logger): Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}