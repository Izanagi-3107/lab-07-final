// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

using LabW07.Models;
using LabW07.Repositories;

namespace LabW07.Demo;

public class SampleData
{
    public InMemoryRepository<ProjectorRemote> Projectors { get; } = new();
    public InMemoryRepository<AirConditionerRemote> AirConditioners { get; } = new();
    public InMemoryRepository<WirelessMicrophone> Microphones { get; } = new();

    public SampleData()
    {
        Projectors.Add(new ProjectorRemote("PR01", "Dieu khien may chieu 1", "Tu A", 15));
        Projectors.Add(new ProjectorRemote("PR02", "Dieu khien may chieu 2", "Tu A", 60));
        Projectors.Add(new ProjectorRemote("PR03", "Dieu khien may chieu 3", "Tu A", 90));
        AirConditioners.Add(new AirConditionerRemote("AC01", "Dieu khien dieu hoa 1", "Tu B", new[] { "A201", "A202" }));
        AirConditioners.Add(new AirConditionerRemote("AC02", "Dieu khien dieu hoa 2", "Tu B", new[] { "A203", "A204" }));
        Microphones.Add(new WirelessMicrophone("MC01", "Micro 1", "Tu C", 20, 80));
        Microphones.Add(new WirelessMicrophone("MC02", "Micro 2", "Tu C", 55, 60));
        Microphones.Add(new WirelessMicrophone("MC03", "Micro 3", "Tu C", 90, 95));
    }

    public IEnumerable<TeachingDevice> GetDevices()
    {
        return Projectors.GetAll().Cast<TeachingDevice>()
            .Concat(AirConditioners.GetAll()).Concat(Microphones.GetAll());
    }
}
