using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Contexts;

public interface IMessagingContextFactory
{
    MessagingContext Create(IMessage message);
}