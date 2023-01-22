using System.Runtime.Serialization;
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

    public bool TryGetContext(IMessage message, out MessagingContext messagingContext)
    {
        try
        {
            var msg = _messageSerializer.Deserialize(message.Payload);
            var constructedType = typeof(MessagingContext<>).MakeGenericType(msg.GetType());
            var instance = (MessagingContext)Activator.CreateInstance(constructedType, msg, message.Topic, message.ReplyTopic, message.CorrelationId);
            messagingContext = instance;
            return true;
        }
        catch (SerializationException ex)
        {
            _logger.LogError(ex, "Message type could not be retrieved.");
            messagingContext = null;
            return false;
        }
    }
}