using System.ComponentModel.DataAnnotations.Schema;
using ProgramPlatform.Enums;

namespace ProgramPlatform.Models;

public class AccountModel
{
    public Guid Id { get; set; }
    public string ReferenceNumber { get; set; }
    public string Name { get; set; }
    public AccountTypeEnum AccountType { get; set; }
    public int UserLimit { get; set; }
    public DateTime SubscriptionExpiry { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    
    [ForeignKey("RoleModel")]
    public Guid RoleId { get; set; }
    public RoleModel Role { get; set; }
    
    public PreferredModeEnum PreferredMode { get; set; }
    public bool MultiFactor { get; set; }
    public bool RoleManagement { get; set; }
    public bool IsArchived { get; set; }
}