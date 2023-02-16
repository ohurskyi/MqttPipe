using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace MqttPipe.Tests.Modules;

public class MqttTestConfiguration : TestcontainerMessageBrokerConfiguration
{
    private const string MqttImage = "eclipse-mosquitto:1.6";
    private const int MqttPort = 1883;

    public MqttTestConfiguration() : base(MqttImage, MqttPort)
    {
    }

    public MqttTestConfiguration(int port): base(MqttImage, MqttPort, port)
    {
        
    }

    public override IWaitForContainerOS WaitStrategy => Wait.ForUnixContainer().UntilPortIsAvailable(DefaultPort);
}