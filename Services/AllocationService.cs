// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

using LabW07.Interfaces;
using LabW07.Models;

namespace LabW07.Services;

public class AllocationService<T> where T : TeachingDevice
{
    private readonly IRepository<T> repository;
    private ISelectionStrategy<T> strategy;

    public AllocationService(IRepository<T> repository, ISelectionStrategy<T> strategy)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(strategy);
        this.repository = repository;
        this.strategy = strategy;
    }

    public void ChangeStrategy(ISelectionStrategy<T> newStrategy)
    {
        ArgumentNullException.ThrowIfNull(newStrategy);
        strategy = newStrategy;
    }

    public T Allocate(Predicate<T>? additionalCondition = null)
    {
        var candidates = repository.FindAll(device => device.IsReadyForUse()
            && (additionalCondition == null || additionalCondition(device)));
        T selected = strategy.Select(candidates);
        selected.Borrow();
        return selected;
    }
}
