using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Definitions.Subscriptions;
using MessagingLibrary.Core.Factory;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Clients;

public class MqttTopicClient<TMessagingClientOptions> : ITopicClient<TMessagingClientOptions> where TMessagingClientOptions : class, IMqttMessagingClientOptions
{
    private readonly IMqttMessagingClient<TMessagingClientOptions> _mqttMessagingClient;
    private readonly IMessageHandlerFactory<TMessagingClientOptions> _messageHandlerFactory;

    public MqttTopicClient(IMqttMessagingClient<TMessagingClientOptions> mqttMessagingClient, IMessageHandlerFactory<TMessagingClientOptions> messageHandlerFactory)
    {
        _mqttMessagingClient = mqttMessagingClient;
        _messageHandlerFactory = messageHandlerFactory;
    }

    public async Task Subscribe(ISubscriptionDefinition subscriptionDefinition)
    {
        await SubscribeInner(subscriptionDefinition);
    }

    public async Task Subscribe(IEnumerable<ISubscriptionDefinition> definitions)
    {
        foreach (var definition in definitions)
        {
            await Subscribe(definition);
        }
    }

    public async Task Unsubscribe(ISubscriptionDefinition definition)
    {
        await UnsubscribeInner(definition);
    }

    public async Task Unsubscribe(IEnumerable<ISubscriptionDefinition> definitions)
    {
        foreach (var definition in definitions)
        {
            await Unsubscribe(definition);
        }
    }

    private async Task UnsubscribeInner(ISubscriptionDefinition subscriptionDefinition)
    {
        if (_messageHandlerFactory.RemoveHandler(subscriptionDefinition.HandlerType, subscriptionDefinition.Topic) == 0)
        {
            await _mqttMessagingClient.UnsubscribeAsync(subscriptionDefinition.Topic);
        }
    }

    private async Task SubscribeInner(ISubscriptionDefinition subscriptionDefinition)
    {
        if (_messageHandlerFactory.RegisterHandler(subscriptionDefinition.HandlerType, subscriptionDefinition.Topic) == 1)
        {
            await _mqttMessagingClient.SubscribeAsync(subscriptionDefinition.Topic);
        }
    }
}