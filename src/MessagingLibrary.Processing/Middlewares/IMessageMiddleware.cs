using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Processing.Middlewares;

public delegate Task<HandlerResult> MessageHandlerDelegate();

public interface IMessageMiddleware<TMessagingClientOptions> where TMessagingClientOptions : IMessagingClientOptions
{
    Task<HandlerResult> Handle(MessagingContext context, MessageHandlerDelegate next);
}

public interface IMessageMiddlewareGeneric<T> where T : class, IMessageContract
{
    Task<HandlerResult> Handle<TMessagingClientOptions>(MessagingContext<T> context, MessageHandlerDelegate next)
        where TMessagingClientOptions : class, IMessagingClientOptions;
}