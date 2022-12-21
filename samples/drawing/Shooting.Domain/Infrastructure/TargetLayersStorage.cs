using System.Collections.Concurrent;
using Shooting.Domain.Models;

namespace Shooting.Domain.Infrastructure;

public class TargetLayersStorage
{
    private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, TargetLayer>> _targetLayersStorage = new();

    public void AddOrUpdate(int lane, TargetLayer targetLayer)
    {
        if(!_targetLayersStorage.TryGetValue(lane, out var layers))
        {
            _targetLayersStorage.TryAdd(lane, new ConcurrentDictionary<int, TargetLayer> { [targetLayer.Index] = targetLayer });
            return;
        }

        layers[targetLayer.Index] = targetLayer;
    }

    public List<TargetLayer> GetTargetLayers(int lane)
    {
        return _targetLayersStorage.TryGetValue(lane, out var layers)
            ? layers.Values.ToList()
            : new List<TargetLayer>();
    }
}