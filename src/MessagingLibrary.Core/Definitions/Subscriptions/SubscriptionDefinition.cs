using MessagingLibrary.Core.Handlers;

namespace MessagingLibrary.Core.Definitions.Subscriptions;

public class SubscriptionDefinition<T> : ISubscriptionDefinition where T: class, IMessageHandler
{
    public SubscriptionDefinition(string topic)
    {
        HandlerType = typeof(T);
        Topic = topic;
    }

    public Type HandlerType { get; }
    public string Topic { get; }
}