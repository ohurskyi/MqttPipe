using System.Collections.Concurrent;

namespace MqttPipe.Clients.RequestResponse.Completion;

public class PendingResponseTracker
{
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<string>> _completionSources = new();

    public Task<string> AddCompletion(Guid correlationId)
    {
        var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        _completionSources.TryAdd(correlationId, tcs);
        return tcs.Task;
    }

    public TaskCompletionSource<string> GetCompletion(Guid correlationId)
    {
        return _completionSources.TryGetValue(correlationId, out var completionSource) ? completionSource : null;
    }

    public void RemoveCompletion(Guid correlationId)
    {
        _completionSources.TryRemove(correlationId, out _);
    }
}