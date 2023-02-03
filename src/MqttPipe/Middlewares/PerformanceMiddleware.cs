using System.Diagnostics;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class PerformanceMiddleware<T> : IMessageMiddleware<T> where T : class, IMessageContract
{
    private readonly ILogger<PerformanceMiddleware<T>> _logger;

    public PerformanceMiddleware(ILogger<PerformanceMiddleware<T>> logger)
    {
        _logger = logger;
    }

    public async Task<HandlerResult> Handle<TMessagingClientOptions>(MessagingContext<T> context, MessageHandlerDelegate<T> next) where TMessagingClientOptions : class, IMessagingClientOptions
    {
        var sw = Stopwatch.StartNew();
        var result = await next(context);
        var el = sw.Elapsed;
        if (el.TotalMilliseconds > 666)
        {
            _logger.LogWarning("Execution time of {msg} - {total}ms take longer than usual.", typeof(T).Name, el.TotalMilliseconds);
            return result;
        }

        _logger.LogDebug("Execution time of {msg} take: {total}ms.", typeof(T).Name, el.TotalMilliseconds);
        return result;
    }
}