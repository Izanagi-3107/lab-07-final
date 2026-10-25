// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

using System.Text;
using LabW07.Demo;
using LabW07.Tests;

namespace LabW07;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        if (args.Contains("--test"))
            return TestRunner.Run();
        if (args.Contains("--demo"))
        {
            DemoRunner.Run();
            return 0;
        }

        while (true)
        {
            Console.WriteLine("\nLAB W07 - Ngô Ngọc Thái - 202418984");
            Console.WriteLine("1. Chay minh hoa");
            Console.WriteLine("2. Chay kiem thu");
            Console.WriteLine("0. Thoat");
            Console.Write("Lua chon: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1": DemoRunner.Run(); break;
                case "2": TestRunner.Run(); break;
                case "0":
                case null: return 0;
                default: Console.WriteLine("Lua chon khong hop le."); break;
            }
        }
    }
}
