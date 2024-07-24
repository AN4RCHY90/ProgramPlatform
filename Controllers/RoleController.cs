using Microsoft.AspNetCore.Mvc;

namespace CirrusCopy.Controllers;

public class RoleController(ILogger<RoleController> logger): Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}