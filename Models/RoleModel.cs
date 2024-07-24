using ProgramPlatform.Enums;

namespace ProgramPlatform.Models;

public class RoleModel
{
    public Guid Id { get; set; }
    public required string RoleName { get; set; }
    public UserTypeEnum UserType { get; set; }
    public AccessLevelEnum PullDeviceProgramming { get; set; }
    public bool ApiAccess { get; set; }
    public bool DashboardAccess { get; set; }
    public bool ProgrammingRollBack { get; set; }
    public AccessLevelEnum ActivityLogPermissions { get; set; }
    public AccessLevelEnum CallPointPermissions { get; set; }
    public AccessLevelEnum IncomingCallPermissions { get; set; }
    public AccessLevelEnum TimePeriodPermissions { get; set; }
    public AccessLevelEnum RelayOpsPermissions { get; set; }
    public AccessLevelEnum SmsCommandsPermissions { get; set; }
    public AccessLevelEnum AlertPermissions { get; set; }
    public AccessLevelEnum PanelSettingsPermissions { get; set; }
    public bool IsArchived { get; set; }
}