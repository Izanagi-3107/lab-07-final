// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

namespace LabW07.Interfaces;

public interface ISelectionStrategy<T>
{
    T Select(IReadOnlyList<T> candidates);
}
