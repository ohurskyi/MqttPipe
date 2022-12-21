using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Definitions.Subscriptions;

namespace MessagingLibrary.Core.Clients;

public interface ITopicClient<TMessagingClientOptions> where TMessagingClientOptions : IMessagingClientOptions
{
    Task Subscribe(ISubscriptionDefinition definition);
    Task Unsubscribe(ISubscriptionDefinition subscriptionDefinition);
}