using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class UnhandledExceptionMiddleware<T> : IMessageMiddleware<T> where T : class, IMessageContract
{
    private readonly ILogger<UnhandledExceptionMiddleware<T>> _logger;

    public UnhandledExceptionMiddleware(ILogger<UnhandledExceptionMiddleware<T>> logger)
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