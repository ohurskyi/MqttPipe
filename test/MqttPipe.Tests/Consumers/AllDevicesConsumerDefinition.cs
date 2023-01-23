using System.Collections.Generic;
using MessagingLibrary.Core.Definitions.Consumers;
using MessagingLibrary.Core.Definitions.Subscriptions;
using MqttPipe.Tests.Handlers;
using MqttPipe.Tests.Topics;

namespace MqttPipe.Tests.Consumers;

public class AllDevicesConsumerDefinition : IConsumerDefinition
{
    private const string MultiWildCardDeviceTopic = $"{DeviceTopicConstants.DeviceTopic}/#";
    
    public IEnumerable<ISubscriptionDefinition> Definitions() => new List<ISubscriptionDefinition>
    {
        new SubscriptionDefinition<HandlerForAllDeviceNumbers>(MultiWildCardDeviceTopic)
    };
}