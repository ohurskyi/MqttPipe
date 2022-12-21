using MessagingLibrary.Core.Definitions.Consumers;

namespace DistributedConfiguration.Domain.Consumers;

public class PairingConsumerDefinitionProvider : IConsumerDefinitionProvider
{
    public IEnumerable<IConsumerDefinition> Definitions => new List<IConsumerDefinition>
    {
        new DevicePairingConsumerDefinition()
    };
}