// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

namespace LabW07.Models;

public class WirelessMicrophone : TeachingDevice
{
    public int BatteryLevel { get; }
    public int SignalQuality { get; }

    public WirelessMicrophone(string id, string name, string location,
        int batteryLevel, int signalQuality) : base(id, name, location)
    {
        BatteryLevel = ValidatePercentage(batteryLevel, nameof(batteryLevel));
        SignalQuality = ValidatePercentage(signalQuality, nameof(signalQuality));
    }

    public override bool IsReadyForUse()
    {
        return Status == DeviceStatus.Available && BatteryLevel >= 30 && SignalQuality >= 40;
    }

    public override double GetDefaultPriorityScore()
    {
        return (BatteryLevel + SignalQuality) / 2.0;
    }
}
