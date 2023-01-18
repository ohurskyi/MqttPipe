using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Definitions.Subscriptions;
using MessagingLibrary.Core.Extensions;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Serialization;
using MqttPipe.Clients.RequestResponse.Handlers;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Clients.RequestResponse;

public class Requester<TMessagingClientOptions> where TMessagingClientOptions : IMqttMessagingClientOptions
{
    private readonly IMessageBus<TMessagingClientOptions> _mqttMessageBus;
    private readonly ITopicClient<TMessagingClientOptions> _mqttTopicClient;
    private readonly PendingResponseTracker _pendingResponseTracker;
    private readonly IMessageSerializer _messageSerializer;

    public Requester(
        IMessageBus<TMessagingClientOptions> mqttMessageBus, 
        ITopicClient<TMessagingClientOptions> mqttTopicClient,
        PendingResponseTracker pendingResponseTracker, 
        IMessageSerializer messageSerializer)
    {
        _mqttMessageBus = mqttMessageBus;
        _pendingResponseTracker = pendingResponseTracker;
        _messageSerializer = messageSerializer;
        _mqttTopicClient = mqttTopicClient;
    }

    public async Task<TMessageResponse> Request<TMessageRequest, TMessageResponse>(string requestTopic, string responseTopic, TMessageRequest contract, TimeSpan timeout)
        where TMessageRequest: class, IMessageRequest
        where TMessageResponse : class, IMessageResponse
    {
        var correlationId = Guid.NewGuid();
        var replyTopic = $"{responseTopic}/{correlationId}";
        var subscription = new SubscriptionDefinition<ResponseHandler>(replyTopic);
        await _mqttTopicClient.Subscribe(subscription);
        
        try
        {
            var message = new Message { Topic = requestTopic, ReplyTopic = replyTopic, CorrelationId = correlationId, Payload = _messageSerializer.Serialize(contract) };
            var responseTask = await PublishAndWaitForCompletion(message);
            var delayTask = Task.Delay(timeout);
            
            if (await Task.WhenAny(responseTask, delayTask) == delayTask)
            {
                throw new OperationCanceledException();
            }

            var response = responseTask.Result;
            return response as TMessageResponse;
        }
        finally
        {
            await _mqttTopicClient.Unsubscribe(subscription);
            _pendingResponseTracker.RemoveCompletion(correlationId);
        }
    }
    
    private async Task<Task<IMessageContract>> PublishAndWaitForCompletion(IMessage message)
    {
        var tcs = _pendingResponseTracker.AddCompletion(message.CorrelationId);
        await _mqttMessageBus.Publish(message);
        return tcs;
    }
}