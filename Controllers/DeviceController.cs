using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProgramPlatform.Controllers;

[Authorize]
public class DeviceController(ILogger<DeviceController> logger): Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}