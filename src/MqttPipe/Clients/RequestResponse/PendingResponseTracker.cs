using System.Collections.Concurrent;
using MessagingLibrary.Core.Messages;

namespace MqttPipe.Clients.RequestResponse;

public class PendingResponseTracker<T> where T : class, IMessageContract
{
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<T>> _completionSources = new();
    
    public Task<T> AddCompletion(Guid correlationId)
    {
        var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        _completionSources.TryAdd(correlationId, tcs);
        return tcs.Task;
    }
    
    public TaskCompletionSource<T> GetCompletion(Guid correlationId)
    {
        return _completionSources.TryGetValue(correlationId, out var completionSource) ? completionSource : null;
    }
    
    public void RemoveCompletion(Guid correlationId)
    {
        _completionSources.TryRemove(correlationId, out _);
    }
}