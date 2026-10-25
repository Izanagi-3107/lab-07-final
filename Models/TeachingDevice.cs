// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

using LabW07.Exceptions;
using LabW07.Interfaces;

namespace LabW07.Models;

public abstract class TeachingDevice : IIdentifiable
{
    public string Id { get; }
    public string Name { get; }
    public string StorageLocation { get; }
    public DeviceStatus Status { get; protected set; }

    protected TeachingDevice(string id, string name, string storageLocation)
    {
        Id = ValidateText(id, nameof(id));
        Name = ValidateText(name, nameof(name));
        StorageLocation = ValidateText(storageLocation, nameof(storageLocation));
        Status = DeviceStatus.Available;
    }

    protected static string ValidateText(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Gia tri khong duoc de trong.", parameterName);
        return value.Trim();
    }

    protected static int ValidatePercentage(int value, string parameterName)
    {
        if (value < 0 || value > 100)
            throw new ArgumentOutOfRangeException(parameterName, "Gia tri phai tu 0 den 100.");
        return value;
    }

    public abstract bool IsReadyForUse();
    public abstract double GetDefaultPriorityScore();

    public void Borrow()
    {
        if (!IsReadyForUse())
            throw new DeviceNotReadyException(Id, Status);
        Status = DeviceStatus.Borrowed;
    }

    public void Return()
    {
        if (Status != DeviceStatus.Borrowed)
            throw new InvalidOperationException($"Thiet bi {Id} chua duoc muon.");
        Status = DeviceStatus.Available;
    }

    public void StartMaintenance()
    {
        if (Status != DeviceStatus.Available)
            throw new InvalidOperationException("Chi bao tri thiet bi dang o trong kho.");
        Status = DeviceStatus.UnderMaintenance;
    }

    public void FinishMaintenance()
    {
        if (Status != DeviceStatus.UnderMaintenance)
            throw new InvalidOperationException("Thiet bi khong trong trang thai bao tri.");
        Status = DeviceStatus.Available;
    }

    public override string ToString()
    {
        return $"{Id,-5} | {Name,-22} | {Status,-16} | {StorageLocation}";
    }
}
