// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

using LabW07.Exceptions;
using LabW07.Interfaces;

namespace LabW07.Strategies;

public class HighestPriorityStrategy<T> : ISelectionStrategy<T>
{
    private readonly Func<T, double> scoreSelector;

    public HighestPriorityStrategy(Func<T, double> scoreSelector)
    {
        ArgumentNullException.ThrowIfNull(scoreSelector);
        this.scoreSelector = scoreSelector;
    }

    public T Select(IReadOnlyList<T> candidates)
    {
        ArgumentNullException.ThrowIfNull(candidates);
        if (candidates.Count == 0)
            throw new NoSuitableItemException();

        T best = candidates[0];
        double bestScore = scoreSelector(best);
        for (int i = 1; i < candidates.Count; i++)
        {
            double score = scoreSelector(candidates[i]);
            if (score > bestScore)
            {
                best = candidates[i];
                bestScore = score;
            }
        }
        return best;
    }
}
