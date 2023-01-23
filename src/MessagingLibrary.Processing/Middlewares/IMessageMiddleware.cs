using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Processing.Middlewares;

public delegate Task<HandlerResult> MessageHandlerDelegate();

public interface IMessageMiddleware<T> where T : class, IMessageContract
{
    Task<HandlerResult> Handle<TMessagingClientOptions>(MessagingContext<T> context, MessageHandlerDelegate next)
        where TMessagingClientOptions : class, IMessagingClientOptions;
}