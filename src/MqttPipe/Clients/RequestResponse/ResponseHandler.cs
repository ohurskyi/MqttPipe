using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MqttPipe.Clients.RequestResponse;

public class ResponseHandler<T> : IMessageHandler<T> where T: class, IMessageResponse
{
    private readonly PendingResponseTracker<T> _pendingResponseTracker;

    public ResponseHandler(PendingResponseTracker<T> pendingResponseTracker)
    {
        _pendingResponseTracker = pendingResponseTracker;
    }

    public async Task<IExecutionResult> HandleAsync(MessagingContext<T> messagingContext)
    {
        var taskCompletionSource = _pendingResponseTracker.GetCompletion(messagingContext.CorrelationId);

        if (taskCompletionSource == null)
        {
            return await Task.FromResult(FailedResult.Create($"Cannot complete the response. Non existing correlation id {messagingContext.CorrelationId}"));
        }

        taskCompletionSource.TrySetResult(messagingContext.Message);
            
        return await Task.FromResult(new SuccessfulResult());
    }
}