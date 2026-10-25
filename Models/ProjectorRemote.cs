// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

namespace LabW07.Models;

public class ProjectorRemote : TeachingDevice
{
    public int BatteryLevel { get; }

    public ProjectorRemote(string id, string name, string location, int batteryLevel)
        : base(id, name, location)
    {
        BatteryLevel = ValidatePercentage(batteryLevel, nameof(batteryLevel));
    }

    public override bool IsReadyForUse()
    {
        return Status == DeviceStatus.Available && BatteryLevel >= 20;
    }

    public override double GetDefaultPriorityScore() => BatteryLevel;
}
