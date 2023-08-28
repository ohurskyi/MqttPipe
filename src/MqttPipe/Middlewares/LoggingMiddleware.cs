using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Middlewares;

public class LoggingMiddleware<T, V> : IMessageMiddleware<T, V> 
    where T : class, IMessageContract
    where V: class, IMqttMessagingClientOptions
{
    private readonly ILogger<LoggingMiddleware<T, V>> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware<T, V>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle(MessagingContext<T> context, V messagingClientOptions, MessageHandlerDelegate<T, V> next)
    {
        var msgType = typeof(T).Name;
        var topic = context.Topic;
        _logger.LogDebug("Begin handling {msg} on topic: {topic} from client {clientAddress}", msgType, topic, messagingClientOptions.MqttBrokerConnectionOptions.Host);
        var result = await next(context, messagingClientOptions);
        _logger.LogDebug("End handling {msg} on topic: {topic} from client {clientAddress}", msgType, topic, messagingClientOptions.MqttBrokerConnectionOptions.Host);
        return result;
    }
}