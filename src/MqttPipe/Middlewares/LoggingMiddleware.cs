using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class LoggingMiddleware<T> : IMessageMiddleware<T> where T : class, IMessageContract
{
    private readonly ILogger<LoggingMiddleware<T>> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware<T>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle<TMessagingClientOptions>(MessagingContext<T> context, MessageHandlerDelegate<T> next) 
        where TMessagingClientOptions : class, IMessagingClientOptions
    {
        var msgType = typeof(T).Name;
        var topic = context.Topic;
        _logger.LogDebug("Begin {msg} handling from topic: {topic}", msgType, topic);
        var result = await next(context);
        _logger.LogDebug("End {msg} handling from topic: {topic}", msgType, topic);
        return result;
    }
}