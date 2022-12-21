using System.Collections.Concurrent;

namespace Shooting.Domain.Infrastructure;

public class ActiveShootingLanesStorage
{
    private readonly ConcurrentDictionary<int, byte> _activeLanesStorage = new();

    public int IncrementActivateLanesCount(int lane)
    {
        if (_activeLanesStorage.ContainsKey(lane))
        {
            return _activeLanesStorage.Count;
        }

        _activeLanesStorage.TryAdd(lane, default);
        
        return _activeLanesStorage.Count;
    }
}