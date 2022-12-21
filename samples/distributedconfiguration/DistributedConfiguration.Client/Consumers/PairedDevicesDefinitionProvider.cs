using MessagingLibrary.Core.Definitions.Consumers;

namespace DistributedConfiguration.Client.Consumers;

public class PairedDevicesDefinitionProvider : IConsumerDefinitionProvider
{
    public IEnumerable<IConsumerDefinition> Definitions => new List<IConsumerDefinition>
    {
        new PairedDevicesConsumerDefinition()
    };
}