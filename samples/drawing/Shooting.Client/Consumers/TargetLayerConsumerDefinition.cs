using MessagingLibrary.Core.Definitions.Consumers;
using MessagingLibrary.Core.Definitions.Subscriptions;
using Shooting.Client.Handlers.TargetLayersCreated;
using Shooting.Contracts.Topics;

namespace Shooting.Client.Consumers;

public class TargetLayerConsumerDefinition : IConsumerDefinition
{
    public IEnumerable<ISubscriptionDefinition> Definitions() => new List<ISubscriptionDefinition>
    {
        new SubscriptionDefinition<TargetLayerCreatedMessageHandler>(ShootingTopicConstants.TargetCreatedTopic)
    };
}