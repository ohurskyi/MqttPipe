namespace MqttPipe.Configuration.Configuration;

public abstract class BaseMqttMessagingClientOptions : IMqttMessagingClientOptions
{
    public MqttBrokerConnectionOptions MqttBrokerConnectionOptions { get; set; } = new();
}