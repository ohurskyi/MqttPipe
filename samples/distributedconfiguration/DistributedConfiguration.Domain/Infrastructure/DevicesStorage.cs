using System.Collections.Concurrent;
using DistributedConfiguration.Contracts.Models;

namespace DistributedConfiguration.Domain.Infrastructure;

public class DevicesStorage
{
    private readonly ConcurrentDictionary<string, Device> _pairedDevicesStorage = new();

    public bool Exists(string deviceMacAddress) => _pairedDevicesStorage.ContainsKey(deviceMacAddress);

    public void Add(Device device)
    {
        _pairedDevicesStorage.TryAdd(device.MacAddress, device);
    }

    public IReadOnlyCollection<Device> GetAll() => _pairedDevicesStorage.Values.ToList();
}