using System.Collections.Concurrent;
using MessagingLibrary.Core.Messages;

namespace MqttPipe.Clients.RequestResponse;

public class PendingResponseTracker
{
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<IMessageContract>> _completionSources = new();
    
    public Task<IMessageContract> AddCompletion(Guid correlationId)
    {
        var tcs = new TaskCompletionSource<IMessageContract>(TaskCreationOptions.RunContinuationsAsynchronously);
        _completionSources.TryAdd(correlationId, tcs);
        return tcs.Task;
    }
    
    public TaskCompletionSource<IMessageContract> GetCompletion(Guid correlationId)
    {
        return _completionSources.TryGetValue(correlationId, out var completionSource) ? completionSource : null;
    }
    
    public void RemoveCompletion(Guid correlationId)
    {
        _completionSources.TryRemove(correlationId, out _);
    }
}