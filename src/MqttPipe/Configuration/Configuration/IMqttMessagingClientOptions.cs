using MessagingLibrary.Core.Configuration;

namespace MqttPipe.Configuration.Configuration
{
    public interface IMqttMessagingClientOptions : IMessagingClientOptions
    {
        MqttBrokerConnectionOptions MqttBrokerConnectionOptions { get; set; }
    }

    public class BaseMqttMessagingClientOptions : IMqttMessagingClientOptions
    {
        public MqttBrokerConnectionOptions MqttBrokerConnectionOptions { get; set; } = new();
    }
}