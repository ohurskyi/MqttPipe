using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Core.Handlers;

public interface IMessageHandler
{
    Task<IExecutionResult> Handle(IMessage message);
}