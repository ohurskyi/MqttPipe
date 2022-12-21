using MQTTnet.Extensions.ManagedClient;

namespace MqttPipe.Configuration.Configuration;

public interface IClientOptionsBuilder<in TMessagingClientOptions> where TMessagingClientOptions : IMqttMessagingClientOptions
{
    ManagedMqttClientOptions BuildManagedMqttClientOptions();
}