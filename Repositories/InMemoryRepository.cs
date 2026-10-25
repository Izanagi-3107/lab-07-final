// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

using LabW07.Exceptions;
using LabW07.Interfaces;

namespace LabW07.Repositories;

public class InMemoryRepository<T> : IRepository<T> where T : class, IIdentifiable
{
    private readonly List<T> items = new();

    private static string NormalizeId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ma khong duoc de trong.", nameof(id));
        return id.Trim();
    }

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        string id = NormalizeId(item.Id);
        if (items.Any(existing => NormalizeId(existing.Id) == id))
            throw new DuplicateIdException(id);
        items.Add(item);
    }

    public bool Remove(string id)
    {
        id = NormalizeId(id);
        int index = items.FindIndex(item => NormalizeId(item.Id) == id);
        if (index < 0)
            return false;
        items.RemoveAt(index);
        return true;
    }

    public T? FindById(string id)
    {
        id = NormalizeId(id);
        return items.Find(item => NormalizeId(item.Id) == id);
    }

    public IReadOnlyList<T> GetAll() => items.ToList().AsReadOnly();

    public IReadOnlyList<T> FindAll(Predicate<T> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);
        return items.FindAll(condition).AsReadOnly();
    }
}
