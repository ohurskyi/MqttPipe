using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Processing.Middlewares;

public interface IPipeline
{
    Task Process(MessagingContext messagingContext, IMessagingClientOptions messagingClientOptions);
}

public interface IPipeline<T,V> : IPipeline
    where T : class, IMessageContract
    where V: class, IMessagingClientOptions
{
    Task IPipeline.Process(MessagingContext messagingContext, IMessagingClientOptions messagingClientOptions)
        => Process((MessagingContext<T>)messagingContext, (V)messagingClientOptions);

    Task Process(MessagingContext<T> messagingContext, V messagingClientOptions);
}