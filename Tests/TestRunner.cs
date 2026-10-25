// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

using LabW07.Demo;
using LabW07.Exceptions;
using LabW07.Models;
using LabW07.Repositories;
using LabW07.Services;
using LabW07.Strategies;

namespace LabW07.Tests;

public static class TestRunner
{
    private static int passed;
    private static int failed;

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }

    private static TException Expect<TException>(Action action) where TException : Exception
    {
        try { action(); }
        catch (TException ex) { return ex; }
        throw new Exception($"Khong phat sinh {typeof(TException).Name}.");
    }

    private static void Test(string name, Action action)
    {
        try
        {
            action();
            passed++;
            Console.WriteLine($"PASS | {name}");
        }
        catch (Exception ex)
        {
            failed++;
            Console.WriteLine($"FAIL | {name} | {ex.Message}");
        }
    }

    private static ProjectorRemote Remote(int battery = 70, string id = "P")
        => new(id, "May chieu", "Tu A", battery);

    private static AllocationService<ProjectorRemote> ProjectorService(SampleData data)
        => new(data.Projectors, new HighestPriorityStrategy<ProjectorRemote>(d => d.GetDefaultPriorityScore()));

    public static int Run()
    {
        passed = failed = 0;
        Console.WriteLine("KIEM THU LAB W07 - Ngô Ngọc Thái - 202418984");
        Test("01 Them du 3 may chieu, 2 dieu hoa, 3 micro", () =>
        {
            var d = new SampleData();
            Check(d.Projectors.GetAll().Count == 3 && d.AirConditioners.GetAll().Count == 2
                && d.Microphones.GetAll().Count == 3, "Sai so luong.");
        });
        Test("02 Trung ma phat sinh DuplicateIdException", () =>
        {
            var d = new SampleData();
            var ex = Expect<DuplicateIdException>(() => d.Projectors.Add(Remote(50, "PR01")));
            Check(ex.DuplicateId == "PR01" && d.Projectors.GetAll().Count == 3, "Kho bi thay doi.");
        });
        Test("03 Tim theo ma va ma khong ton tai", () =>
        {
            var d = new SampleData();
            Check(d.Projectors.FindById("PR02")?.BatteryLevel == 60
                && d.Projectors.FindById("X") == null, "Tim sai.");
        });
        Test("04 Loc thiet bi san sang", () =>
        {
            var d = new SampleData();
            Check(d.Projectors.FindAll(x => x.IsReadyForUse()).Select(x => x.Id)
                .SequenceEqual(new[] { "PR02", "PR03" }), "Loc sai.");
        });
        Test("05 May chieu diem cao nhat la PR03", () =>
        {
            var d = new SampleData();
            Check(ProjectorService(d).Allocate().Id == "PR03", "Chon sai.");
        });
        Test("06 Dieu hoa cho phong A203 la AC02", () =>
        {
            var d = new SampleData();
            var s = new AllocationService<AirConditionerRemote>(d.AirConditioners,
                new HighestPriorityStrategy<AirConditionerRemote>(x => x.GetDefaultPriorityScore()));
            Check(s.Allocate(x => x.IsCompatibleWith("A203")).Id == "AC02", "Sai phong.");
        });
        Test("07 Micro dau tien san sang la MC02", () =>
        {
            var d = new SampleData();
            var s = new AllocationService<WirelessMicrophone>(d.Microphones,
                new FirstAvailableStrategy<WirelessMicrophone>());
            Check(s.Allocate().Id == "MC02", "Chon sai.");
        });
        Test("08 Doi chien luoc tren cung tap ung vien", () =>
        {
            var d = new SampleData();
            var s = new AllocationService<WirelessMicrophone>(d.Microphones,
                new FirstAvailableStrategy<WirelessMicrophone>());
            var first = s.Allocate();
            first.Return();
            s.ChangeStrategy(new HighestPriorityStrategy<WirelessMicrophone>(x => x.GetDefaultPriorityScore()));
            Check(first.Id == "MC02" && s.Allocate().Id == "MC03", "Doi chien luoc sai.");
        });
        Test("09 Khong co ung vien va kho giu nguyen", () =>
        {
            var d = new SampleData();
            Expect<NoSuitableItemException>(() => ProjectorService(d).Allocate(x => false));
            Check(d.Projectors.GetAll().All(x => x.Status == DeviceStatus.Available), "Doi trang thai sai.");
        });
        Test("10 Cap phat cap nhat Borrowed trong kho", () =>
        {
            var d = new SampleData();
            var selected = ProjectorService(d).Allocate();
            Check(ReferenceEquals(selected, d.Projectors.FindById(selected.Id))
                && selected.Status == DeviceStatus.Borrowed && !selected.IsReadyForUse(), "Sai trang thai.");
        });
        Test("11 Cung repository generic cho nhieu loai", () =>
        {
            var mixed = new InMemoryRepository<TeachingDevice>();
            mixed.Add(Remote());
            mixed.Add(new WirelessMicrophone("M", "Micro", "Tu C", 80, 90));
            Check(mixed.FindAll(x => x.IsReadyForUse()).Count == 2, "Kho chung sai.");
        });
        Test("12 Tra thiet bi va muon lai", () =>
        {
            var r = Remote(); r.Borrow(); r.Return();
            Check(r.Status == DeviceStatus.Available, "Tra sai.");
            r.Borrow(); Check(r.Status == DeviceStatus.Borrowed, "Khong muon lai duoc.");
        });
        Test("13 Muon thiet bi pin thap", () =>
        {
            var r = Remote(19);
            var ex = Expect<DeviceNotReadyException>(r.Borrow);
            Check(ex.DeviceId == "P" && ex.CurrentStatus == DeviceStatus.Available
                && r.Status == DeviceStatus.Available, "Sai ngoai le/trang thai.");
        });
        Test("14 Muon lap va tra lap", () =>
        {
            var r = Remote();
            Expect<InvalidOperationException>(r.Return);
            r.Borrow(); Expect<DeviceNotReadyException>(r.Borrow);
            r.Return(); Expect<InvalidOperationException>(r.Return);
            Check(r.Status == DeviceStatus.Available, "Sai trang thai.");
        });
        Test("15 Bao tri va ket thuc bao tri", () =>
        {
            var r = Remote(); r.StartMaintenance();
            Check(!r.IsReadyForUse(), "Bao tri van san sang.");
            Expect<DeviceNotReadyException>(r.Borrow);
            r.FinishMaintenance(); Check(r.IsReadyForUse(), "Khong san sang sau bao tri.");
        });
        Test("16 Nguong pin may chieu 19 va 20", () =>
            Check(!Remote(19).IsReadyForUse() && Remote(20).IsReadyForUse(), "Sai nguong."));
        Test("17 Nguong micro 29 30 va 39 40", () =>
        {
            Check(!new WirelessMicrophone("A", "M", "T", 29, 40).IsReadyForUse(), "Sai nguong pin.");
            Check(!new WirelessMicrophone("B", "M", "T", 30, 39).IsReadyForUse(), "Sai nguong tin hieu.");
            Check(new WirelessMicrophone("C", "M", "T", 30, 40).IsReadyForUse(), "Sai nguong hop le.");
        });
        Test("18 Diem micro khong bi chia nguyen", () =>
            Check(new WirelessMicrophone("M", "M", "T", 55, 60).GetDefaultPriorityScore() == 57.5, "Chia nguyen."));
        Test("19 Tu choi pin va tin hieu ngoai 0 den 100", () =>
        {
            Expect<ArgumentOutOfRangeException>(() => Remote(-1));
            Expect<ArgumentOutOfRangeException>(() => Remote(101));
            Expect<ArgumentOutOfRangeException>(() => new WirelessMicrophone("M", "M", "T", -1, 50));
            Expect<ArgumentOutOfRangeException>(() => new WirelessMicrophone("M", "M", "T", 50, 101));
        });
        Test("20 Tu choi chuoi trong va chuan hoa khoang trang", () =>
        {
            Expect<ArgumentException>(() => Remote(50, " "));
            Expect<ArgumentException>(() => new ProjectorRemote("P", " ", "T", 50));
            Expect<ArgumentException>(() => new ProjectorRemote("P", "P", " ", 50));
            Check(Remote(50, " P ").Id == "P", "Chua Trim.");
        });
        Test("21 Repository tu choi null", () =>
            Expect<ArgumentNullException>(() => new InMemoryRepository<ProjectorRemote>().Add(null!)));
        Test("22 Xoa ton tai va khong ton tai", () =>
        {
            var d = new SampleData();
            Check(d.Projectors.Remove("PR01") && !d.Projectors.Remove("PR01")
                && d.Projectors.FindById("PR01") == null, "Xoa sai.");
        });
        Test("23 GetAll khong lo collection noi bo", () =>
        {
            var d = new SampleData(); var snapshot = d.Projectors.GetAll();
            Expect<NotSupportedException>(() => ((IList<ProjectorRemote>)snapshot).Clear());
            d.Projectors.Remove("PR01");
            Check(snapshot.Count == 3 && d.Projectors.GetAll().Count == 2, "Khong phai ban sao.");
        });
        Test("24 FindAll bao ve collection va tu choi null", () =>
        {
            var d = new SampleData();
            var filtered = d.Projectors.FindAll(x => true);
            Expect<NotSupportedException>(() => ((IList<ProjectorRemote>)filtered).Clear());
            Expect<ArgumentNullException>(() => d.Projectors.FindAll(null!));
        });
        Test("25 Hai chien luoc deu bao loi danh sach rong", () =>
        {
            Expect<NoSuitableItemException>(() => new FirstAvailableStrategy<int>().Select(Array.Empty<int>()));
            Expect<NoSuitableItemException>(() => new HighestPriorityStrategy<int>(x => x).Select(Array.Empty<int>()));
        });
        Test("26 Hoa diem giu phan tu dau va khong sua dau vao", () =>
        {
            var list = new List<ProjectorRemote> { Remote(80, "A"), Remote(80, "B") };
            var strategy = new HighestPriorityStrategy<ProjectorRemote>(x => x.BatteryLevel);
            Check(strategy.Select(list).Id == "A" && list.Select(x => x.Id).SequenceEqual(new[] { "A", "B" }), "Sai hoa diem.");
            Check(new FirstAvailableStrategy<ProjectorRemote>().Select(list).Id == "A" && list.Count == 2, "Sua danh sach.");
        });
        Test("27 Tu choi cac dependency null", () =>
        {
            var d = new SampleData(); var strategy = new FirstAvailableStrategy<ProjectorRemote>();
            Expect<ArgumentNullException>(() => new AllocationService<ProjectorRemote>(null!, strategy));
            Expect<ArgumentNullException>(() => new AllocationService<ProjectorRemote>(d.Projectors, null!));
            Expect<ArgumentNullException>(() => ProjectorService(d).ChangeStrategy(null!));
            Expect<ArgumentNullException>(() => new HighestPriorityStrategy<int>(null!));
        });
        Test("28 Phong tuong thich khong bi sua tu ben ngoai", () =>
        {
            var rooms = new HashSet<string> { "A203" };
            var r = new AirConditionerRemote("A", "Dieu hoa", "Tu", rooms);
            rooms.Clear(); Check(r.IsCompatibleWith("A203") && !r.IsCompatibleWith("A999"), "Lo tap phong.");
            Expect<ArgumentException>(() => new AirConditionerRemote("B", "D", "T", Array.Empty<string>()));
            Expect<ArgumentException>(() => new AirConditionerRemote("B", "D", "T", new[] { " " }));
        });
        Test("29 Khong cap phat lai thiet bi dang muon", () =>
        {
            var d = new SampleData(); var s = ProjectorService(d);
            Check(s.Allocate().Id == "PR03" && s.Allocate().Id == "PR02", "Cap phat lap.");
            Expect<NoSuitableItemException>(() => s.Allocate());
        });
        Test("30 Chi loc dieu kien bo sung sau khi san sang", () =>
        {
            var d = new SampleData(); int evaluated = 0;
            var selected = ProjectorService(d).Allocate(x => { evaluated++; return x.BatteryLevel <= 70; });
            Check(evaluated == 2 && selected.Id == "PR02", "Thu tu loc sai.");
        });
        Test("31 Loi predicate khong lam muon thiet bi", () =>
        {
            var d = new SampleData();
            Expect<InvalidOperationException>(() => ProjectorService(d).Allocate(x => throw new InvalidOperationException()));
            Check(d.Projectors.GetAll().All(x => x.Status == DeviceStatus.Available), "Doi trang thai sai.");
        });
        Test("32 Kho rong va ma khong hop le", () =>
        {
            var r = new InMemoryRepository<ProjectorRemote>();
            var s = new AllocationService<ProjectorRemote>(r, new FirstAvailableStrategy<ProjectorRemote>());
            Expect<NoSuitableItemException>(() => s.Allocate());
            Expect<ArgumentException>(() => r.FindById(" "));
            Expect<ArgumentException>(() => r.Remove(" "));
        });
        Test("33 Bao tri sai trang thai khong doi du lieu", () =>
        {
            var r = Remote(); r.Borrow();
            Expect<InvalidOperationException>(r.StartMaintenance);
            Expect<InvalidOperationException>(r.FinishMaintenance);
            Check(r.Status == DeviceStatus.Borrowed, "Sai trang thai.");
        });
        Test("34 Da hinh hanh vi va diem qua TeachingDevice", () =>
        {
            TeachingDevice[] items = { Remote(15), new WirelessMicrophone("M", "M", "T", 55, 60),
                new AirConditionerRemote("A", "A", "T", new[] { "A203" }) };
            Check(!items[0].IsReadyForUse() && items[1].IsReadyForUse() && items[2].IsReadyForUse(), "Sai dispatch.");
            Check(items.Select(x => x.GetDefaultPriorityScore()).SequenceEqual(new[] { 15.0, 57.5, 100.0 }), "Sai diem.");
        });
        Console.WriteLine($"TONG KET: {passed} PASS, {failed} FAIL");
        return failed == 0 ? 0 : 1;
    }
}
