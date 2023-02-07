using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Factory;

public interface IMessagingContextFactory
{
    bool TryGetContext(IMessage message, out MessagingContext messagingContext, out string serializedMessageType);
}