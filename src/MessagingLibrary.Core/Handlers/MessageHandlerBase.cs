using MessagingLibrary.Core.Extensions;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Core.Handlers;

public abstract class MessageHandlerNew<T> : IMessageHandler where T: class, IMessageContract
{
    public Task<IExecutionResult> Handle(object ctx)
    {
        return HandleAsync((MessagingContextNew<T>)ctx);
    }
    
    protected abstract Task<IExecutionResult> HandleAsync(MessagingContextNew<T> messagingContext);
}

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
    
    public Task<IExecutionResult> Handle(object ctx)
    {
        throw new NotImplementedException();
    }
}