using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Core.Handlers;

public interface IMessageHandlerGeneric<T> where T : class, IMessageContract
{
    Task<IExecutionResult> HandleAsync(MessagingContext<T> messagingContext);
}