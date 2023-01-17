using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Serialization;

namespace MessagingLibrary.Core.Factory;

public interface IMessagingContextNew
{
    
}

public class MessagingContextNew<T> : IMessagingContextNew where T: class, IMessageContract
{
    public MessagingContextNew(T message)
    {
        Message = message;
    }

    public T Message { get; }
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
        var instance = (IMessagingContextNew)Activator.CreateInstance(constructedType, msg);
        return instance;
    }
}