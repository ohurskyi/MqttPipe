using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Definitions.Subscriptions;
using MessagingLibrary.Core.Extensions;
using MessagingLibrary.Core.Messages;
using MqttPipe.Clients.RequestResponse.Completion;
using MqttPipe.Clients.RequestResponse.Handlers;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Clients.RequestResponse;

public class Requester<TMessagingClientOptions> where TMessagingClientOptions : IMqttMessagingClientOptions
{
    private readonly IMessageBus<TMessagingClientOptions> _mqttMessageBus;
    private readonly ITopicClient<TMessagingClientOptions> _mqttTopicClient;
    private readonly PendingResponseTracker _pendingResponseTracker;

    public Requester(
        IMessageBus<TMessagingClientOptions> mqttMessageBus, 
        ITopicClient<TMessagingClientOptions> mqttTopicClient,
        PendingResponseTracker pendingResponseTracker)
    {
        _mqttMessageBus = mqttMessageBus;
        _pendingResponseTracker = pendingResponseTracker;
        _mqttTopicClient = mqttTopicClient;
    }

    public async Task<string> Request(string requestTopic, string responseTopic, IMessageContract contract, TimeSpan timeout)
    {
        var correlationId = Guid.NewGuid();
        var replyTopic = $"{responseTopic}/{correlationId}";
        var subscription = new SubscriptionDefinition<ResponseHandler>(replyTopic);
        await _mqttTopicClient.Subscribe(subscription);
        
        try
        {
            var message = new Message { Topic = requestTopic, ReplyTopic = replyTopic, CorrelationId = correlationId, Payload = contract.MessagePayloadToJson() };
            var responseTask = await PublishAndWaitForCompletion(message);
            var delayTask = Task.Delay(timeout);
            
            if (await Task.WhenAny(responseTask, delayTask) == delayTask)
            {
                throw new OperationCanceledException();
            }

            return responseTask.Result;
        }
        finally
        {
            await _mqttTopicClient.Unsubscribe(subscription);
            _pendingResponseTracker.RemoveCompletion(correlationId);
        }
    }
    
    private async Task<Task<string>> PublishAndWaitForCompletion(IMessage message)
    {
        var tcs = _pendingResponseTracker.AddCompletion(message.CorrelationId);
        await _mqttMessageBus.Publish(message);
        return tcs;
    }
}