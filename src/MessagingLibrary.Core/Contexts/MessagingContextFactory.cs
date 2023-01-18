using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Serialization;

namespace MessagingLibrary.Core.Contexts;

public class MessagingContextFactory : IMessagingContextFactory
{
    private readonly IMessageSerializer _messageSerializer;

    public MessagingContextFactory(IMessageSerializer messageSerializer)
    {
        _messageSerializer = messageSerializer;
    }

    public MessagingContext Create(IMessage message)
    {
        var msg = _messageSerializer.Deserialize(message.Payload);
        var constructedType = typeof(MessagingContext<>).MakeGenericType(msg.GetType());
        var instance = (MessagingContext)Activator.CreateInstance(constructedType, msg, message.Topic, message.ReplyTopic, message.CorrelationId);
        return instance;
    }
}