using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Tests.Modules;

public class MqttTestContainer : TestcontainerMessageBroker
{
    public MqttTestContainer(IContainerConfiguration configuration, ILogger logger) : base(configuration, logger)
    {
    }
}