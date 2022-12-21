using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Clients;

public interface IMessageBus<TMessagingClientOptions> where TMessagingClientOptions : class, IMessagingClientOptions
{
    Task Publish(IMessageContract contract, string topic);
    Task Publish(IMessage message);
}