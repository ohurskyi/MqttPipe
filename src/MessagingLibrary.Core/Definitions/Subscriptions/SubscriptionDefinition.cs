using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Definitions.Subscriptions;

public class SubscriptionDefinition<TMessage, THandler> : ISubscriptionDefinition 
    where TMessage: class, IMessageContract
    where THandler: IMessageHandlerGeneric<TMessage>
{
    public SubscriptionDefinition(string topic)
    {
        HandlerType = typeof(THandler);
        Topic = topic;
    }

    public Type HandlerType { get; }
    public string Topic { get; }
}