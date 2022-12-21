using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Serialization;
using MqttPipe.Extensions;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Clients;

public class MqttMessageBus<TMessagingClientOptions> : IMessageBus<TMessagingClientOptions> where TMessagingClientOptions : class, IMqttMessagingClientOptions
{
    private readonly IMqttMessagingClient<TMessagingClientOptions> _mqttMessagingClient;
    private readonly IMessageSerializer _messageSerializer;

    public MqttMessageBus(IMqttMessagingClient<TMessagingClientOptions> mqttMessagingClient, IMessageSerializer messageSerializer)
    {
        _mqttMessagingClient = mqttMessagingClient;
        _messageSerializer = messageSerializer;
    }

    public async Task Publish(IMessageContract contract, string topic)
    {
        var message = new Message { Topic = topic, Payload = _messageSerializer.Serialize(contract) };
        await _mqttMessagingClient.PublishAsync(message.ToMqttMessage());
    }

    public async Task Publish(IMessage message)
    {
        await _mqttMessagingClient.PublishAsync(message.ToMqttMessage());
    }
}