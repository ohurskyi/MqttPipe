using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Infrastructure;

public class InfrastructureClientOptions : IMqttMessagingClientOptions
{
    public MqttBrokerConnectionOptions MqttBrokerConnectionOptions { get; set; } = new() { Host = "localhost", Port = 1883 };
}