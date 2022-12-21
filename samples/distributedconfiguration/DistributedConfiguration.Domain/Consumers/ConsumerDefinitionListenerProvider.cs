using MessagingLibrary.Core.Clients;
using MessagingLibrary.Processing.Listeners;
using MqttPipe.Infrastructure;

namespace DistributedConfiguration.Domain.Consumers;

public class ConsumerDefinitionListenerProvider : IConsumerDefinitionListenerProvider
{
    private readonly ITopicClient<InfrastructureClientOptions> _topicClient;

    public ConsumerDefinitionListenerProvider(ITopicClient<InfrastructureClientOptions> topicClient)
    {
        _topicClient = topicClient;
    }

    public IEnumerable<IConsumerListener> Listeners => new List<IConsumerListener>
    {
        new ConsumerListener<InfrastructureClientOptions>(_topicClient, new PairingConsumerDefinitionProvider())
    };
}