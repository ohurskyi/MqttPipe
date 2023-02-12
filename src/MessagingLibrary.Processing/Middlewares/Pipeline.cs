using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Processing.Strategy;

namespace MessagingLibrary.Processing.Middlewares;

public class Pipeline<T, V> : IPipeline<T, V> 
    where T : class, IMessageContract
    where V: class, IMessagingClientOptions
{
    private readonly IMessageMiddleware<T, V>[] _middlewares;
    private readonly MessageHandlingStrategy<T, V> _strategy;

    public Pipeline(IEnumerable<IMessageMiddleware<T, V>> middlewares, MessageHandlingStrategy<T, V> strategy)
    {
        _strategy = strategy;
        _middlewares = middlewares.ToArray();
    }

    public async Task Process(MessagingContext<T> messagingContext, V messagingClientOptions)
    {
        MessageHandlerDelegate<T, V> lastPipe = _strategy.HandleAsync;
        var messageHandler = BuildPipeline(_middlewares, lastPipe);
        await messageHandler(messagingContext, messagingClientOptions);
    }

    private static MessageHandlerDelegate<T, V> BuildPipeline(
        IMessageMiddleware<T, V>[] middlewares,
        MessageHandlerDelegate<T, V> @delegate)
    {
        for (var i = middlewares.Length - 1; i >= 0; i--)
        {
            var middleware = middlewares[i];
            MessageHandlerDelegate<T, V> Wrap(MessageHandlerDelegate<T, V> next) => (context, clientOptions) => 
                middleware.Handle(context, clientOptions, next);
            @delegate = Wrap(@delegate);
        }

        return @delegate;
    }
}