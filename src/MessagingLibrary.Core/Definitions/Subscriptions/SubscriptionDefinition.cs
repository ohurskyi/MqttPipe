using MessagingLibrary.Core.Handlers;

namespace MessagingLibrary.Core.Definitions.Subscriptions;

public class SubscriptionDefinition<THandler> : ISubscriptionDefinition
    where THandler: IMessageHandler
{
    public SubscriptionDefinition(string topic)
    {
        HandlerType = typeof(THandler);
        Topic = topic;
    }

    public Type HandlerType { get; }
    public string Topic { get; }
}