using MessagingLibrary.Core.Definitions.Consumers;

namespace Shooting.Domain.Consumers;

public class ConsumerDefinitionProvider : IConsumerDefinitionProvider
{
    public IEnumerable<IConsumerDefinition> Definitions => new List<IConsumerDefinition>
    {
        new ShootingInfoConsumerDefinition()
    };
}