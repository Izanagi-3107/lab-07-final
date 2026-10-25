// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

namespace LabW07.Exceptions;

public class NoSuitableItemException : Exception
{
    public NoSuitableItemException() : base("Khong co thiet bi phu hop de cap phat.") { }
}
