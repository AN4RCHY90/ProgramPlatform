using Microsoft.AspNetCore.Mvc;

namespace ProgramPlatform.Controllers;

public class DeviceController(ILogger<DeviceController> logger): Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}