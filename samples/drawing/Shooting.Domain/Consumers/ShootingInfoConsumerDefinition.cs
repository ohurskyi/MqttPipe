using MessagingLibrary.Core.Definitions.Consumers;
using MessagingLibrary.Core.Definitions.Subscriptions;
using Shooting.Contracts;
using Shooting.Contracts.Topics;
using Shooting.Domain.Handlers.GetTargetLayers;
using Shooting.Domain.Handlers.ShootingInfo;

namespace Shooting.Domain.Consumers;

public class ShootingInfoConsumerDefinition : IConsumerDefinition
{
    public IEnumerable<ISubscriptionDefinition> Definitions()
    {
        return new List<ISubscriptionDefinition>
        {
            new SubscriptionDefinition<CreateTargetLayerMessageHandler>($"{ShootingTopicConstants.ShootingInfoTopic}"),
            new SubscriptionDefinition<CreateHitLayerMessageHandler>($"{ShootingTopicConstants.ShootingInfoTopic}"),
            new SubscriptionDefinition<CreateInfoLayerMessageHandler>($"{ShootingTopicConstants.ShootingInfoTopic}"),
            new SubscriptionDefinition<RecordShootingSessionStartedMessageHandler>($"{ShootingTopicConstants.ShootingInfoTopic}"),
            
            new SubscriptionDefinition<GetTargetLayersMessageHandler>($"{ShootingTopicConstants.ShootingInfoRequest}")
        };
    }
}