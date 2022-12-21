using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Serialization;

namespace MessagingLibrary.Core.Factory;

public class MessagingContextFactory : IMessagingContextFactory
{
    private readonly IMessageSerializer _messageSerializer;

    public MessagingContextFactory(IMessageSerializer messageSerializer)
    {
        _messageSerializer = messageSerializer;
    }

    public bool TryGetContext(IMessage message, out MessagingContext messagingContext, out string serializedMessageType)
    {
        var deserializedContract = _messageSerializer.Deserialize(message.Payload);
        if (deserializedContract.Message == null)
        {
            messagingContext = null;
            serializedMessageType = deserializedContract.Type;
            return false;
        }

        var messageContract = deserializedContract.Message;
        var constructedType = typeof(MessagingContext<>).MakeGenericType(messageContract.GetType());
        var instance = (MessagingContext)Activator.CreateInstance(constructedType, messageContract, message.Topic, message.ReplyTopic, message.CorrelationId);
        messagingContext = instance;
        serializedMessageType = deserializedContract.Type;
        return true;
    }
}