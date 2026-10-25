// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

using System.Collections.Frozen;

namespace LabW07.Models;

public class AirConditionerRemote : TeachingDevice
{
    public IReadOnlySet<string> CompatibleRooms { get; }

    public AirConditionerRemote(string id, string name, string location,
        IEnumerable<string> compatibleRooms) : base(id, name, location)
    {
        ArgumentNullException.ThrowIfNull(compatibleRooms);
        CompatibleRooms = compatibleRooms
            .Select(room => ValidateText(room, nameof(compatibleRooms)))
            .ToFrozenSet(StringComparer.Ordinal);
        if (CompatibleRooms.Count == 0)
            throw new ArgumentException("Can it nhat mot phong tuong thich.", nameof(compatibleRooms));
    }

    public bool IsCompatibleWith(string roomId)
    {
        return CompatibleRooms.Contains(ValidateText(roomId, nameof(roomId)));
    }

    public override bool IsReadyForUse() => Status == DeviceStatus.Available;

    public override double GetDefaultPriorityScore() => 100;
}
