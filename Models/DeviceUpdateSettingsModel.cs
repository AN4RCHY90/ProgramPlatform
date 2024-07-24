namespace ProgramPlatform.Models;

public class DeviceUpdateSettingsModel
{
    public Guid Id { get; set; }
    public int NumberOfRetries { get; set; }
    public int RetryInterval { get; set; }
    public DateTime UpdateTime { get; set; }
    public int ProgrammingMethodThresold { get; set; }
}