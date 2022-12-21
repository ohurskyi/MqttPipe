using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class UnhandledExceptionMiddleware<T, V> : IMessageMiddleware<T, V> 
    where T : class, IMessageContract
    where V: class, IMessagingClientOptions
{
    private readonly ILogger<UnhandledExceptionMiddleware<T, V>> _logger;

    public UnhandledExceptionMiddleware(ILogger<UnhandledExceptionMiddleware<T, V>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle(MessagingContext<T> context, V messagingClientOptions, MessageHandlerDelegate<T, V> next)
    {
        try
        {
            return await next(context, messagingClientOptions);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled Exception while processing message: {msg} on topic {topicValue}",
                typeof(T).Name, context.Topic);
            throw;
        }
    }
}