using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Results;

namespace MqttPipe.Clients.RequestResponse.Handlers;

public class ResponseHandler : IMessageHandler
{
    private readonly PendingResponseTracker _pendingResponseTracker;

    public ResponseHandler(PendingResponseTracker pendingResponseTracker)
    {
        _pendingResponseTracker = pendingResponseTracker;
    }

    public async Task<IExecutionResult> Handle(object ctx)
    {
        var messagingContext = ctx as MessagingContext;

        if (messagingContext == null)
        {
            return await Task.FromResult(FailedResult.Create("Cannot restore messaging context."));
        }
        
        var taskCompletionSource = _pendingResponseTracker.GetCompletion(messagingContext.CorrelationId);

        if (taskCompletionSource == null)
        {
            return await Task.FromResult(FailedResult.Create($"Cannot complete the response. Non existing correlation id {messagingContext.CorrelationId}"));
        }

        taskCompletionSource.TrySetResult(messagingContext.Message);
            
        return await Task.FromResult(new SuccessfulResult());
    }
}