using MessagingLibrary.Core.Definitions.Consumers;

namespace Shooting.Client.Consumers;

public class ConsumerDefinitionProvider : IConsumerDefinitionProvider
{
    public IEnumerable<IConsumerDefinition> Definitions => new List<IConsumerDefinition>
    {
        new TargetLayerConsumerDefinition()
    };
}