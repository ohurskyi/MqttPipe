using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Core.Handlers;

public abstract class MessageHandler<T> : IMessageHandlerGeneric<T> where T: class, IMessageContract
{
    public abstract Task<IExecutionResult> HandleAsync(MessagingContext<T> messagingContext);
}