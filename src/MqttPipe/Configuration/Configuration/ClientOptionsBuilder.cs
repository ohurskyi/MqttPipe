using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;

namespace MqttPipe.Configuration.Configuration;

public abstract class ClientOptionsBuilder<TMessagingClientOptions> : IClientOptionsBuilder<TMessagingClientOptions>
    where TMessagingClientOptions : IMqttMessagingClientOptions
{
    private readonly MqttClientOptionsBuilder _mqttClientOptionsBuilder;

    private readonly ManagedMqttClientOptionsBuilder _managedMqttClientOptionsBuilder;

    protected ClientOptionsBuilder(TMessagingClientOptions clientOptions)
    {
        _mqttClientOptionsBuilder = new MqttClientOptionsBuilder()
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .WithTcpServer(clientOptions.MqttBrokerConnectionOptions.Host,
                clientOptions.MqttBrokerConnectionOptions.Port);

        _managedMqttClientOptionsBuilder = new ManagedMqttClientOptionsBuilder();
    }

    

    ManagedMqttClientOptions IClientOptionsBuilder<TMessagingClientOptions>.BuildManagedMqttClientOptions()
    {
        var clientOptions = BuildClientOptions(_mqttClientOptionsBuilder);
        return BuildManagedMqttClientOptions(_managedMqttClientOptionsBuilder.WithClientOptions(clientOptions));
    }

    protected abstract MqttClientOptions BuildClientOptions(MqttClientOptionsBuilder mqttClientOptionsBuilder);

    protected abstract ManagedMqttClientOptions BuildManagedMqttClientOptions(ManagedMqttClientOptionsBuilder managedMqttClientOptionsBuilder);
}