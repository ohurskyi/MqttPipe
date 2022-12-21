using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Tests.Options;

public class TestMessagingClientOptions : IMqttMessagingClientOptions
{
    public MqttBrokerConnectionOptions MqttBrokerConnectionOptions { get; set; }
}