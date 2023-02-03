using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Processing.Strategy;

namespace MessagingLibrary.Processing.Middlewares;

public interface IPipeline
{
    Task Process<TMessagingClientOptions>(object ctx) where TMessagingClientOptions : class, IMessagingClientOptions;
}

public interface IPipeline<T> : IPipeline
    where T : class, IMessageContract
{
    Task IPipeline.Process<TMessagingClientOptions>(object ctx)
    {
        return Process<TMessagingClientOptions>((MessagingContext<T>)ctx);
    }

    Task Process<TMessagingClientOptions>(MessagingContext<T> messagingContext)
        where TMessagingClientOptions : class, IMessagingClientOptions;
}

public class Pipeline<T> : IPipeline<T> where T : class, IMessageContract
{
    private readonly IMessageMiddleware<T>[] _middlewares;
    private readonly MessageHandlingStrategy<T> _strategy;

    public Pipeline(IEnumerable<IMessageMiddleware<T>> middlewares, MessageHandlingStrategy<T> strategy)
    {
        _strategy = strategy;
        _middlewares = middlewares.ToArray();
    }

    public async Task Process<TMessagingClientOptions>(MessagingContext<T> messagingContext)  
        where TMessagingClientOptions : class, IMessagingClientOptions
    {
        MessageHandlerDelegate<T> lastPipe = _strategy.HandleAsync<TMessagingClientOptions>;
        var messageHandler = BuildPipeline<TMessagingClientOptions>(_middlewares, lastPipe);
        await messageHandler(messagingContext);
    }

    private static MessageHandlerDelegate<T> BuildPipeline<TMessagingClientOptions>(
        IMessageMiddleware<T>[] middlewares,
        MessageHandlerDelegate<T> @delegate)
        where TMessagingClientOptions : class, IMessagingClientOptions
    {
        for (var i = middlewares.Length - 1; i >= 0; i--)
        {
            var middleware = middlewares[i];
            Func<MessageHandlerDelegate<T>, MessageHandlerDelegate<T>> wrap = next => 
                context => middleware.Handle<TMessagingClientOptions>(context, next);

            @delegate = wrap(@delegate);
        }

        return @delegate;
    }
}