using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Infrastructure;

public class InfrastructureClientOptions : BaseMqttMessagingClientOptions
{
    public InfrastructureClientOptions()
    {
        MqttBrokerConnectionOptions = new() { Host = "localhost", Port = 1883 };
    }
}