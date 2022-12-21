using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class UnhandledExceptionMiddleware<TMessagingClientOptions> : IMessageMiddleware<TMessagingClientOptions>  
    where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly ILogger<UnhandledExceptionMiddleware<TMessagingClientOptions>> _logger;

    public UnhandledExceptionMiddleware(ILogger<UnhandledExceptionMiddleware<TMessagingClientOptions>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle(IMessage message, MessageHandlerDelegate next)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled Exception while processing message on topic {topicValue}", message.Topic);
            throw;
        }
    }
}