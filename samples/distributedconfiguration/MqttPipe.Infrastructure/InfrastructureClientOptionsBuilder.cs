using MqttPipe.Configuration.Configuration;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace MqttPipe.Infrastructure;

public class InfrastructureClientOptionsBuilder : ClientOptionsBuilder<InfrastructureClientOptions>
{
    public InfrastructureClientOptionsBuilder(InfrastructureClientOptions clientOptions) : base(clientOptions)
    {
    }
    
    protected override MqttClientOptions BuildClientOptions(MqttClientOptionsBuilder mqttClientOptionsBuilder)
    {
        return mqttClientOptionsBuilder
            .WithClientId($"infrastructure_{Guid.NewGuid()}")
            .Build();
    }

    protected override ManagedMqttClientOptions BuildManagedMqttClientOptions(ManagedMqttClientOptionsBuilder managedMqttClientOptionsBuilder)
    {
        return managedMqttClientOptionsBuilder
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
            .Build();
    }
}