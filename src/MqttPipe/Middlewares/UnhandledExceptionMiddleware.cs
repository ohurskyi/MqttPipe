using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class UnhandledExceptionMiddlewareGeneric<T> : IMessageMiddlewareGeneric<T> where T : class, IMessageContract
{
    private readonly ILogger<UnhandledExceptionMiddlewareGeneric<T>> _logger;

    public UnhandledExceptionMiddlewareGeneric(ILogger<UnhandledExceptionMiddlewareGeneric<T>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle<TMessagingClientOptions>(MessagingContext<T> context, MessageHandlerDelegate next)
        where TMessagingClientOptions : class, IMessagingClientOptions
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled Exception while processing message: {msg} on topic {topicValue}",
                typeof(T).Name, context.Topic);
            throw;
        }
    }
}


public class UnhandledExceptionMiddleware<TMessagingClientOptions> : IMessageMiddleware<TMessagingClientOptions>  
    where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly ILogger<UnhandledExceptionMiddleware<TMessagingClientOptions>> _logger;

    public UnhandledExceptionMiddleware(ILogger<UnhandledExceptionMiddleware<TMessagingClientOptions>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle(MessagingContext context, MessageHandlerDelegate next)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled Exception while processing message on topic {topicValue}", context.Topic);
            throw;
        }
    }
}