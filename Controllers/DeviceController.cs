using Microsoft.AspNetCore.Mvc;

namespace CirrusCopy.Controllers;

public class DeviceController(ILogger<DeviceController> logger): Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}