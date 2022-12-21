namespace MessagingLibrary.Core.Definitions.Consumers;

public interface IConsumerDefinitionProvider
{
    IEnumerable<IConsumerDefinition> Definitions { get; }
}