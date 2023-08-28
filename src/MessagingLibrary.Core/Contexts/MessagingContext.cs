using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Contexts;

public abstract class MessagingContext : IResponseContext
{
    protected MessagingContext(IMessageContract message, string topic, string replyTopic, Guid correlationId)
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

public class MessagingContext<T> : MessagingContext 
    where T: class, IMessageContract
{
    public MessagingContext(IMessageContract message, string topic, string replyTopic, Guid correlationId) 
        : base(message, topic, replyTopic, correlationId)
    {
    }
    
    public override T Message => (T)base.Message;
}