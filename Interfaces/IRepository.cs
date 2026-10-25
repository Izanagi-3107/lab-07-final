// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

namespace LabW07.Interfaces;

public interface IRepository<T> where T : class, IIdentifiable
{
    void Add(T item);
    bool Remove(string id);
    T? FindById(string id);
    IReadOnlyList<T> GetAll();
    IReadOnlyList<T> FindAll(Predicate<T> condition);
}
