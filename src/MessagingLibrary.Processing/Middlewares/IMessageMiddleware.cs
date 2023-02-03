using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Processing.Middlewares;

public delegate Task<HandlerResult> MessageHandlerDelegate<T>(MessagingContext<T> context)
    where T : class, IMessageContract;

public interface IMessageMiddleware<T> where T : class, IMessageContract
{
    Task<HandlerResult> Handle<TMessagingClientOptions>(MessagingContext<T> context, MessageHandlerDelegate<T> next)
        where TMessagingClientOptions : class, IMessagingClientOptions;
}