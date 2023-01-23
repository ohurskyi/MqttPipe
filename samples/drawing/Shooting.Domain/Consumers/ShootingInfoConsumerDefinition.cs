using MessagingLibrary.Core.Definitions.Consumers;
using MessagingLibrary.Core.Definitions.Subscriptions;
using Shooting.Contracts;
using Shooting.Contracts.ShootingInfo;
using Shooting.Contracts.TargetLayers;
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
            new SubscriptionDefinition<ShootingInfoContract, CreateTargetLayerMessageHandler>($"{ShootingTopicConstants.ShootingInfoTopic}"),
            new SubscriptionDefinition<ShootingInfoContract, CreateHitLayerMessageHandler>($"{ShootingTopicConstants.ShootingInfoTopic}"),
            new SubscriptionDefinition<ShootingInfoContract, CreateInfoLayerMessageHandler>($"{ShootingTopicConstants.ShootingInfoTopic}"),
            new SubscriptionDefinition<ShootingInfoContract,RecordShootingSessionStartedMessageHandler>($"{ShootingTopicConstants.ShootingInfoTopic}"),
            
            new SubscriptionDefinition<GetTargetLayersRequest, GetTargetLayersMessageHandler>($"{ShootingTopicConstants.ShootingInfoRequest}")
        };
    }
}