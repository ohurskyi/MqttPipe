using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Contexts;

public interface IMessagingContextFactory
{
    bool TryGetContext(IMessage message, out MessagingContext messagingContext);
}