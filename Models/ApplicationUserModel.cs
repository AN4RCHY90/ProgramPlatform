using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ProgramPlatform.Enums;

namespace ProgramPlatform.Models;

public class ApplicationUserModel: IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public PreferredModeEnum PreferredMode { get; set; }
    public bool MultiFactor { get; set; }
    public bool IsAdmin { get; set; }

    // Store UserTypes as a comma-separated string
    public string UserTypes { get; set; }

    [ForeignKey("AccountModels")]
    public Guid AccountId { get; set; }
    public AccountModel Account { get; set; }

    public bool IsArchived { get; set; }

    [NotMapped]
    public List<UserTypeEnum> UserTypesList
    {
        get => UserTypes?.Split(',')?.Select(Enum.Parse<UserTypeEnum>)?.ToList() ?? new List<UserTypeEnum>();
        set => UserTypes = string.Join(',', value);
    }
}