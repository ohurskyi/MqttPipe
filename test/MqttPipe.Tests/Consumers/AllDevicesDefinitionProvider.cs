using System.Collections.Generic;
using MessagingLibrary.Core.Definitions.Consumers;

namespace MqttPipe.Tests.Consumers;

public class AllDevicesDefinitionProvider : IConsumerDefinitionProvider
{
    public IEnumerable<IConsumerDefinition> Definitions => new List<IConsumerDefinition>
    {
        new AllDevicesConsumerDefinition()
    };
}