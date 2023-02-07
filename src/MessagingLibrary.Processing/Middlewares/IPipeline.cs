using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Processing.Middlewares;

public interface IPipeline
{
    Task Process<TMessagingClientOptions>(MessagingContext messagingContext) where TMessagingClientOptions : class, IMessagingClientOptions;
}

public interface IPipeline<T> : IPipeline
    where T : class, IMessageContract
{
    Task IPipeline.Process<TMessagingClientOptions>(MessagingContext messagingContext) 
        => Process<TMessagingClientOptions>((MessagingContext<T>)messagingContext);

    Task Process<TMessagingClientOptions>(MessagingContext<T> messagingContext)
        where TMessagingClientOptions : class, IMessagingClientOptions;
}