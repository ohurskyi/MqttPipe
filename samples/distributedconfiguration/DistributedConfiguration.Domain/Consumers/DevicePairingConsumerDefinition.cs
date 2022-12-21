using DistributedConfiguration.Contracts.Topics;
using DistributedConfiguration.Domain.Handlers;
using MessagingLibrary.Core.Definitions.Consumers;
using MessagingLibrary.Core.Definitions.Subscriptions;

namespace DistributedConfiguration.Domain.Consumers;

public class DevicePairingConsumerDefinition : IConsumerDefinition
{
    public IEnumerable<ISubscriptionDefinition> Definitions()
    {
        return new List<ISubscriptionDefinition>
        {
            new SubscriptionDefinition<PairDeviceMessageHandler>($"{DistributedConfigurationTopicConstants.PairDevice}"),
            new SubscriptionDefinition<GetPairedDeviceMessageHandler>($"{DistributedConfigurationTopicConstants.RequestUpdate}")
        };
    }
}