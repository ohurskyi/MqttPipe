using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Shooting;

public class ShootingClientOptions : IMqttMessagingClientOptions
{
    public MqttBrokerConnectionOptions MqttBrokerConnectionOptions { get; set; } = new();
}