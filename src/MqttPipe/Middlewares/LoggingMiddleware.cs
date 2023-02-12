using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class LoggingMiddleware<T, V> : IMessageMiddleware<T, V> 
    where T : class, IMessageContract
    where V: class, IMessagingClientOptions
{
    private readonly ILogger<LoggingMiddleware<T, V>> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware<T, V>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle(MessagingContext<T> context, MessageHandlerDelegate<T> next)
    {
        var msgType = typeof(T).Name;
        var topic = context.Topic;
        _logger.LogDebug("Begin {msg} handling from topic: {topic}", msgType, topic);
        var result = await next(context);
        _logger.LogDebug("End {msg} handling from topic: {topic}", msgType, topic);
        return result;
    }
}