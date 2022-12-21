using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Processing.Strategy;

public interface IMessageHandlingStrategy<TMessagingClientOptions> where TMessagingClientOptions: IMessagingClientOptions
{
    Task<HandlerResult> Handle(IMessage message);
}