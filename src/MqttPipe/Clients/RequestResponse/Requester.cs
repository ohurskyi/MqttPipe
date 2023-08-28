using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Definitions.Subscriptions;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Serialization;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Clients.RequestResponse;

public class Requester<TMessagingClientOptions, TMessageRequest, TMessageResponse> 
    where TMessagingClientOptions : class, IMqttMessagingClientOptions
    where TMessageRequest: class, IMessageRequest
    where TMessageResponse : class, IMessageResponse
{
    private readonly IMessageBus<TMessagingClientOptions> _mqttMessageBus;
    private readonly ITopicClient<TMessagingClientOptions> _mqttTopicClient;
    private readonly PendingResponseTracker<TMessageResponse> _pendingResponseTracker;
    private readonly IMessageSerializer _messageSerializer;

    public Requester(
        IMessageBus<TMessagingClientOptions> mqttMessageBus, 
        ITopicClient<TMessagingClientOptions> mqttTopicClient,
        PendingResponseTracker<TMessageResponse> pendingResponseTracker, 
        IMessageSerializer messageSerializer)
    {
        _mqttMessageBus = mqttMessageBus;
        _pendingResponseTracker = pendingResponseTracker;
        _messageSerializer = messageSerializer;
        _mqttTopicClient = mqttTopicClient;
    }

    public async Task<TMessageResponse> Request(string requestTopic, string responseTopic, TMessageRequest request, TimeSpan timeout)
    {
        var correlationId = Guid.NewGuid();
        var replyTopic = $"{responseTopic}/{correlationId}";
        var subscription = new SubscriptionDefinition<ResponseHandler<TMessageResponse>>(replyTopic);
        await _mqttTopicClient.Subscribe(subscription);
        
        try
        {
            var message = new Message { Topic = requestTopic, ReplyTopic = replyTopic, CorrelationId = correlationId, Payload = _messageSerializer.Serialize(request) };
            var responseTask = await PublishAndWaitForCompletion(message);
            var delayTask = Task.Delay(timeout);
            
            if (await Task.WhenAny(responseTask, delayTask) == delayTask)
            {
                throw new OperationCanceledException();
            }

            var response = responseTask.Result;
            return response;
        }
        finally
        {
            await _mqttTopicClient.Unsubscribe(subscription);
            _pendingResponseTracker.RemoveCompletion(correlationId);
        }
    }
    
    private async Task<Task<TMessageResponse>> PublishAndWaitForCompletion(IMessage message)
    {
        var tcs = _pendingResponseTracker.AddCompletion(message.CorrelationId);
        await _mqttMessageBus.Publish(message);
        return tcs;
    }
}