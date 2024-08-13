using System.ComponentModel.DataAnnotations;
using ProgramPlatform.Enums;

namespace ProgramPlatform.ViewModels;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
    public string MobileNumber { get; set; }
    public PreferredModeEnum PreferredMode { get; set; }
    [Required]
    public Guid RoleId { get; set; }
    public bool MultiFactor { get; set; }
    public bool IsAdmin { get; set; }
    public List<UserTypeEnum> UserTypeList { get; set; }
    [Required]
    public Guid AccountId { get; set; }
    public bool IsArchived { get; set; }
}