using ProgramPlatform.Enums;

namespace ProgramPlatform.Models;

public class SimNetworkModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string AccessPointName { get; set; }
    public SimAuthEnum AuthenticationType { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool IsArchived { get; set; }
}