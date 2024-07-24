using Microsoft.AspNetCore.Mvc;

namespace CirrusCopy.Controllers;

public class UserController(ILogger<UserController> logger): Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}