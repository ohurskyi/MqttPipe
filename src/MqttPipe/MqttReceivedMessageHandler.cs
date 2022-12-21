using MessagingLibrary.Processing.Executor;
using MqttPipe.Extensions;
using MQTTnet.Client;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe;

public class MqttReceivedMessageHandler<TMessagingClientOptions>
    where TMessagingClientOptions : class, IMqttMessagingClientOptions
{
    private readonly IMessageExecutor<TMessagingClientOptions> _messageExecutor;

    public MqttReceivedMessageHandler(IMessageExecutor<TMessagingClientOptions> messageExecutor)
    {
        _messageExecutor = messageExecutor;
    }

    public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        await _messageExecutor.ExecuteAsync(eventArgs.ApplicationMessage.ToMessage());
    }
}