using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class LoggingMiddleware<TMessagingClientOptions> : IMessageMiddleware<TMessagingClientOptions>  
    where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly ILogger<LoggingMiddleware<TMessagingClientOptions>> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware<TMessagingClientOptions>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle(IMessagingContextNew context, MessageHandlerDelegate next)
    {
        _logger.LogDebug("Begin message handling on topic {value}", context.Topic);
        var result = await next();
        _logger.LogDebug("End message handling on topic {value}", context.Topic);
        return result;
    }
}