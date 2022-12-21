using MessagingLibrary.Core.Definitions.Subscriptions;

namespace MessagingLibrary.Core.Definitions.Consumers;

public interface IConsumerDefinition
{
    IEnumerable<ISubscriptionDefinition> Definitions();
}