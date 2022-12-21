using DistributedConfiguration.Client.IntegrationEvents.PairedDevicesConfigurationChanged;
using DistributedConfiguration.Contracts.Topics;
using MessagingLibrary.Core.Definitions.Consumers;
using MessagingLibrary.Core.Definitions.Subscriptions;

namespace DistributedConfiguration.Client.Consumers;

public class PairedDevicesConsumerDefinition : IConsumerDefinition
{
    public IEnumerable<ISubscriptionDefinition> Definitions()
    {
        return new List<ISubscriptionDefinition>
        {
            new SubscriptionDefinition<UpdateLocalConfigurationMessageHandler>($"{DistributedConfigurationTopicConstants.CurrentConfiguration}"),
            new SubscriptionDefinition<NotifyUsersMessageHandler>($"{DistributedConfigurationTopicConstants.CurrentConfiguration}")
        };
    }
}