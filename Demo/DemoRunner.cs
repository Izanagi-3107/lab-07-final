// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

using LabW07.Exceptions;
using LabW07.Models;
using LabW07.Services;
using LabW07.Strategies;

namespace LabW07.Demo;

public static class DemoRunner
{
    public static void Run()
    {
        var data = new SampleData();
        Console.WriteLine("=== MINH HOA LAB W07 ===");
        Console.WriteLine("Ngô Ngọc Thái - 202418984");
        foreach (var device in data.GetDevices())
            Console.WriteLine($"{device} | Ready={device.IsReadyForUse()} | Score={device.GetDefaultPriorityScore()}");

        try { data.Projectors.Add(new ProjectorRemote("PR01", "Trung ma", "Tu A", 80)); }
        catch (DuplicateIdException ex) { Console.WriteLine($"Trung ma: {ex.Message}"); }

        Console.WriteLine($"Tim PR02: {data.Projectors.FindById("PR02")?.Name}");
        Console.WriteLine("May chieu san sang: " + string.Join(", ",
            data.Projectors.FindAll(d => d.IsReadyForUse()).Select(d => d.Id)));

        var projectorService = new AllocationService<ProjectorRemote>(data.Projectors,
            new HighestPriorityStrategy<ProjectorRemote>(d => d.GetDefaultPriorityScore()));
        var projector = projectorService.Allocate();
        Console.WriteLine($"Pin cao nhat: {projector.Id}, {projector.Status}");

        var airService = new AllocationService<AirConditionerRemote>(data.AirConditioners,
            new FirstAvailableStrategy<AirConditionerRemote>());
        var air = airService.Allocate(d => d.IsCompatibleWith("A203"));
        Console.WriteLine($"Phong A203: {air.Id}, {air.Status}");

        var microService = new AllocationService<WirelessMicrophone>(data.Microphones,
            new FirstAvailableStrategy<WirelessMicrophone>());
        var first = microService.Allocate();
        Console.WriteLine($"Micro dau tien: {first.Id}, {first.Status}");
        first.Return();
        Console.WriteLine($"Tra {first.Id}: {first.Status}");
        microService.ChangeStrategy(new HighestPriorityStrategy<WirelessMicrophone>(d => d.GetDefaultPriorityScore()));
        var highest = microService.Allocate();
        Console.WriteLine($"Doi sang diem cao nhat: {highest.Id}, {highest.Status}");

        try { airService.Allocate(d => d.IsCompatibleWith("A999")); }
        catch (NoSuitableItemException ex) { Console.WriteLine($"Phong A999: {ex.Message}"); }
        projector.Return();
        Console.WriteLine($"Tra {projector.Id}: {projector.Status}");
    }
}
