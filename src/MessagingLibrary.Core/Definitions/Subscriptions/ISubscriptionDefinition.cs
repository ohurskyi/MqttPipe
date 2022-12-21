namespace MessagingLibrary.Core.Definitions.Subscriptions;

public interface ISubscriptionDefinition
{
    Type HandlerType { get; }
    string Topic { get;}
}