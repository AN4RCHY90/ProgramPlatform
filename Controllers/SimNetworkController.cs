using Microsoft.AspNetCore.Mvc;

namespace CirrusCopy.Controllers;

public class SimNetworkController(ILogger<SimNetworkController> logger): Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}