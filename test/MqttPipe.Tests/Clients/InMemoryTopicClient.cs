using System.Threading.Tasks;
using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Definitions.Subscriptions;
using MessagingLibrary.Core.Factory;

namespace MqttPipe.Tests.Clients;

public class InMemoryTopicClient<TMessagingClientOptions> : ITopicClient<TMessagingClientOptions> where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly IMessageHandlerFactory<TMessagingClientOptions> _messageHandlerFactory;

    public InMemoryTopicClient(IMessageHandlerFactory<TMessagingClientOptions> messageHandlerFactory)
    {
        _messageHandlerFactory = messageHandlerFactory;
    }

    public Task Subscribe(ISubscriptionDefinition definition)
    {
        _messageHandlerFactory.RegisterHandler(definition.HandlerType, definition.Topic);
        return Task.CompletedTask;
    }

    public Task Unsubscribe(ISubscriptionDefinition subscriptionDefinition)
    {
        _messageHandlerFactory.RemoveHandler(subscriptionDefinition.HandlerType, subscriptionDefinition.Topic);
        return Task.CompletedTask;
    }
}