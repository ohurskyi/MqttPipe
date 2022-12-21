using MessagingLibrary.Core.Clients;
using MessagingLibrary.Processing.Listeners;
using MqttPipe.Shooting;

namespace Shooting.Domain.Consumers;

public class ConsumerDefinitionListenerProvider : IConsumerDefinitionListenerProvider
{
    private readonly ITopicClient<ShootingClientOptions> _topicClient;

    public ConsumerDefinitionListenerProvider(ITopicClient<ShootingClientOptions> topicClient)
    {
        _topicClient = topicClient;
    }

    public IEnumerable<IConsumerListener> Listeners => new List<IConsumerListener>
    {
        new ConsumerListener<ShootingClientOptions>(_topicClient, new ConsumerDefinitionProvider())
    };
}