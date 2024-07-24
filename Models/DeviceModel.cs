using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramPlatform.Models;

public class DeviceModel
{
    public Guid Id { get; set; }
    public long DeviceId { get; set; }
    public long Serial { get; set; }
    public string SalesOrder { get; set; }
    public string ModelNo { get; set; }
    public string InstallLocation { get; set; }
    public string InstallAddress { get; set; }
    public string Country { get; set; }
    public string Timezone { get; set; }
    public string UnitPhoneNumber { get; set; }
    public long FirmwareVersion { get; set; }
    public DateTime WarrantyExpiry { get; set; }
    public string InstalledBy { get; set; }
    public DateTime InstallDate { get; set; }
    public string Account { get; set; }
    public bool SiteGrouping { get; set; }
    public bool HubAndSatellite { get; set; }
    public int CallPointLimit { get; set; }
    public int IncomingCallLimit { get; set; }
    public int RelayOpsLimit { get; set; }
    
    [ForeignKey("SimNetworkModel")]
    public Guid SimId { get; set; }
    public SimNetworkModel SimCardNetwork { get; set; }
    
    public bool IsArchived { get; set; }
}