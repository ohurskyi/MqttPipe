using System.Threading.Channels;
using System.Threading.Tasks;
using MessagingLibrary.Core.Messages;

namespace MqttPipe.Tests.Clients;

public class InMemoryMessageChannel
{
    private readonly Channel<IMessage> _channel = Channel.CreateUnbounded<IMessage>();

    public async ValueTask Enqueue(IMessage message)
    {
        await _channel.Writer.WriteAsync(message);
    }

    public async Task<IMessage> Dequeue()
    {
        return await _channel.Reader.ReadAsync();
    }
}