using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Core.Handlers;

public interface IMessageHandler
{
}

public interface IMessageHandler<T> : IMessageHandler
    where T : class, IMessageContract
{
    Task<IExecutionResult> HandleAsync(MessagingContext<T> messagingContext);
}