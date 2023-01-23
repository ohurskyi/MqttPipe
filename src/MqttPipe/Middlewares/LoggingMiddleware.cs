using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class LoggingMiddlewareGeneric<T> : IMessageMiddlewareGeneric<T> where T : class, IMessageContract
{
    private readonly ILogger<LoggingMiddlewareGeneric<T>> _logger;

    public LoggingMiddlewareGeneric(ILogger<LoggingMiddlewareGeneric<T>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle<TMessagingClientOptions>(MessagingContext<T> context, MessageHandlerDelegate next) 
        where TMessagingClientOptions : class, IMessagingClientOptions
    {
        var msgType = typeof(T).Name;
        var topic = context.Topic;
        _logger.LogDebug("Begin message: {msg} handling on topic: {topic}", msgType, topic);
        var result = await next();
        _logger.LogDebug("End message: {msg} handling on topic: {topic}", msgType, topic);
        return result;
    }
}

public class LoggingMiddleware<TMessagingClientOptions> : IMessageMiddleware<TMessagingClientOptions>  
    where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly ILogger<LoggingMiddleware<TMessagingClientOptions>> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware<TMessagingClientOptions>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle(MessagingContext context, MessageHandlerDelegate next)
    {
        _logger.LogDebug("Begin message handling on topic {value}", context.Topic);
        var result = await next();
        _logger.LogDebug("End message handling on topic {value}", context.Topic);
        return result;
    }
}