using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Serialization;
using Microsoft.Extensions.Logging;

namespace MessagingLibrary.Core.Contexts;

public class MessagingContextFactory : IMessagingContextFactory
{
    private readonly IMessageSerializer _messageSerializer;
    private readonly ILogger<MessagingContextFactory> _logger;

    public MessagingContextFactory(IMessageSerializer messageSerializer, ILogger<MessagingContextFactory> logger)
    {
        _messageSerializer = messageSerializer;
        _logger = logger;
    }

    public bool TryGetContext(IMessage message, out MessagingContext messagingContext, out string serializedMessageType)
    {
        var (messageContract, messageType) = _messageSerializer.Deserialize(message.Payload);
        if (messageContract == null)
        {
            messagingContext = null;
            serializedMessageType = messageType;
            return false;
        }   

        var constructedType = typeof(MessagingContext<>).MakeGenericType(messageContract.GetType());
        var instance = (MessagingContext)Activator.CreateInstance(constructedType, messageContract, message.Topic, message.ReplyTopic, message.CorrelationId);
        messagingContext = instance;
        serializedMessageType = messageType;
        return true;
    }
}