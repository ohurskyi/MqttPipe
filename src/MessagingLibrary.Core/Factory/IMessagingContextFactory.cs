using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Serialization;

namespace MessagingLibrary.Core.Factory;

public abstract class IMessagingContextNew
{
    protected IMessagingContextNew(IMessageContract message, string topic, string replyTopic, Guid correlationId)
    {
        Message = message;
        Topic = topic;
        ReplyTopic = replyTopic;
        CorrelationId = correlationId;
    }

    public virtual IMessageContract Message { get; }
    public string Topic { get; }
    public string ReplyTopic { get; }
    public Guid CorrelationId { get; }
}

public class MessagingContextNew<T> : IMessagingContextNew, IResponseContext 
    where T: class, IMessageContract
{
    public MessagingContextNew(IMessageContract message, string topic, string replyTopic, Guid correlationId) : base(message, topic, replyTopic, correlationId)
    {
    }

    public override T Message => (T)base.Message;
}

public interface IMessagingContextFactory
{
    IMessagingContextNew Create(IMessage message);
}

public class MessagingContextFactory : IMessagingContextFactory
{
    private readonly IMessageSerializer _messageSerializer;

    public MessagingContextFactory(IMessageSerializer messageSerializer)
    {
        _messageSerializer = messageSerializer;
    }

    public IMessagingContextNew Create(IMessage message)
    {
        var msg = _messageSerializer.Deserialize(message.Payload);
        var constructedType = typeof(MessagingContextNew<>).MakeGenericType(msg.GetType());
        var instance = (IMessagingContextNew)Activator.CreateInstance(constructedType, msg, message.Topic, message.ReplyTopic, message.CorrelationId);
        return instance;
    }
}