using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Core.Handlers;

public abstract class MessageHandler<T> : IMessageHandler where T: class, IMessageContract
{
    public Task<IExecutionResult> Handle(object ctx)
    {
        return HandleAsync((MessagingContext<T>)ctx);
    }
    
    protected abstract Task<IExecutionResult> HandleAsync(MessagingContext<T> messagingContext);
}