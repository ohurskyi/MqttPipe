using System.Threading.Tasks;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Processing.Executor;

namespace MqttPipe.Tests.Clients;

public class InMemoryMessageReceivedHandler<TMessagingClientOptions>
    where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly IMessageExecutor<TMessagingClientOptions> _messageExecutor;
    private readonly InMemoryMessageChannel _busChannel;

    public InMemoryMessageReceivedHandler(IMessageExecutor<TMessagingClientOptions> messageExecutor, InMemoryMessageChannel busChannel)
    {
        _messageExecutor = messageExecutor;
        _busChannel = busChannel;
    }

    public async Task HandleApplicationMessageReceivedAsync()
    {
        var message = await _busChannel.Dequeue();
        await _messageExecutor.ExecuteAsync(message);
    }
}