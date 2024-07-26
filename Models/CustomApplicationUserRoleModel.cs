using Microsoft.AspNetCore.Identity;

namespace ProgramPlatform.Models;

public class CustomApplicationUserRoleModel
{
    public class ApplicationUser : IdentityUser<Guid> { }
    public class ApplicationRole : IdentityRole<Guid> { }
}