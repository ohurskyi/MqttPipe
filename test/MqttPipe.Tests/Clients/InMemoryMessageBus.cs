using System.Threading.Tasks;
using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Serialization;

namespace MqttPipe.Tests.Clients;

public class InMemoryMessageBus<TMessagingClientOptions> : IMessageBus<TMessagingClientOptions>
    where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly InMemoryMessageChannel _channel;
    private readonly IMessageSerializer _messageSerializer;

    public InMemoryMessageBus(InMemoryMessageChannel channel, IMessageSerializer messageSerializer)
    {
        _channel = channel;
        _messageSerializer = messageSerializer;
    }

    public async Task Publish(IMessageContract contract, string topic)
    { 
        var message = new Message { Topic = topic, Payload = _messageSerializer.Serialize(contract) };
        await _channel.Enqueue(message);
    }

    public async Task Publish(IMessage message)
    {
        await _channel.Enqueue(message);
    }
}