using Microsoft.AspNetCore.Mvc;

namespace ProgramPlatform.Controllers;

public class SimNetworkController(ILogger<SimNetworkController> logger): Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}