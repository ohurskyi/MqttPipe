namespace MessagingLibrary.Processing.Listeners;

public interface IConsumerDefinitionListenerProvider
{
    IEnumerable<IConsumerListener> Listeners { get; }
}