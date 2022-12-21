using System.Collections.Generic;
using MessagingLibrary.Core.Clients;
using MessagingLibrary.Processing.Listeners;
using MqttPipe.Tests.Options;

namespace MqttPipe.Tests.Consumers;

public class ConsumerDefinitionListenerProvider : IConsumerDefinitionListenerProvider
{
    private readonly ITopicClient<TestMessagingClientOptions> _topicClient;

    public ConsumerDefinitionListenerProvider(ITopicClient<TestMessagingClientOptions> topicClient)
    {
        _topicClient = topicClient;
    }

    public IEnumerable<IConsumerListener> Listeners => new List<IConsumerListener>
    {
        new ConsumerListener<TestMessagingClientOptions>(_topicClient, new AllDevicesDefinitionProvider())
    };
}