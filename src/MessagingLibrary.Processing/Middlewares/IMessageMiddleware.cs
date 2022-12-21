using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Processing.Middlewares;

public delegate Task<HandlerResult> MessageHandlerDelegate<T, V>(MessagingContext<T> context, V messagingClientOptions)
    where T : class, IMessageContract
    where V : class, IMessagingClientOptions;

public interface IMessageMiddleware<T, V> 
    where T : class, IMessageContract
    where V: class, IMessagingClientOptions
{
    Task<HandlerResult> Handle(MessagingContext<T> context, V messagingClientOptions, MessageHandlerDelegate<T, V> next);
}