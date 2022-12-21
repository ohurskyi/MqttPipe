using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Extensions;
using MessagingLibrary.Core.Messages;
using MqttPipe.Clients.RequestResponse.Completion;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Clients.RequestResponse;

public class RequestClient<TMessagingClientOptions> : IRequestClient<TMessagingClientOptions> where TMessagingClientOptions : IMqttMessagingClientOptions
{
    private readonly IMessageBus<TMessagingClientOptions> _mqttMessageBus;
    private readonly ITopicClient<TMessagingClientOptions> _mqttTopicClient;
    private readonly PendingResponseTracker _pendingResponseTracker;

    public RequestClient(IMessageBus<TMessagingClientOptions> mqttMessageBus, ITopicClient<TMessagingClientOptions> mqttTopicClient, PendingResponseTracker pendingResponseTracker)
    {
        _mqttMessageBus = mqttMessageBus;
        _mqttTopicClient = mqttTopicClient;
        _pendingResponseTracker = pendingResponseTracker;
    }

    public async Task<TMessageResponse> SendAndWaitAsync<TMessageRequest, TMessageResponse>(string requestTopic, string responseTopic, TMessageRequest contract, TimeSpan timeout) 
        where TMessageRequest: class, IMessageRequest
        where TMessageResponse : class, IMessageResponse
    {
        var requester = new Requester<TMessagingClientOptions>(_mqttMessageBus, _mqttTopicClient, _pendingResponseTracker);
        var response = await requester.Request(requestTopic, responseTopic, contract, timeout);
        var messagePayloadFromJson = response?.MessagePayloadFromJson<TMessageResponse>();
        return messagePayloadFromJson;
    }
}