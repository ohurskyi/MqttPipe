using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Definitions.Subscriptions;

namespace MessagingLibrary.Core.Clients;

public interface ITopicClient<TMessagingClientOptions> where TMessagingClientOptions : IMessagingClientOptions
{
    Task Subscribe(ISubscriptionDefinition definition);
    Task Subscribe(IEnumerable<ISubscriptionDefinition> definitions);
    Task Unsubscribe(ISubscriptionDefinition definition);
    Task Unsubscribe(IEnumerable<ISubscriptionDefinition> definitions);
}