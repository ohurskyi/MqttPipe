using MessagingLibrary.Core.Extensions;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Core.Handlers;

public abstract class MessageHandlerBase<T> : IMessageHandler
    where T: IMessageContract
{
    public Task<IExecutionResult> Handle(IMessage message)
    {
        var context = new MessagingContext<T>
        {
            Topic = message.Topic,
            Payload = message.Payload.MessagePayloadFromJson<T>(),
            ReplyTopic = message.ReplyTopic,
            CorrelationId = message.CorrelationId
        };
        return HandleAsync(context);
    }
    
    protected abstract Task<IExecutionResult> HandleAsync(MessagingContext<T> messagingContext);
}