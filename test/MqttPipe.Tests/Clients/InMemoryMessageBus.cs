using System.Threading.Tasks;
using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Messages;

namespace MqttPipe.Tests.Clients;

public class InMemoryMessageBus<TMessagingClientOptions> : IMessageBus<TMessagingClientOptions>
    where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly InMemoryMessageChannel _channel;

    public InMemoryMessageBus(InMemoryMessageChannel channel)
    {
        _channel = channel;
    }

    public Task Publish(IMessageContract contract, string topic)
    {
        throw new System.NotImplementedException();
    }

    public async Task Publish(IMessage message)
    {
        await _channel.Enqueue(message);
    }
}