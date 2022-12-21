using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Server;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Shooting;

public class ShootingClientOptionsBuilder : ClientOptionsBuilder<ShootingClientOptions>
{
    public ShootingClientOptionsBuilder(ShootingClientOptions clientOptions) : base(clientOptions)
    {
    }

    protected override MqttClientOptions BuildClientOptions(MqttClientOptionsBuilder mqttClientOptionsBuilder)
    {
        return mqttClientOptionsBuilder
            .WithClientId($"shooting_{Guid.NewGuid()}")
            .WithTimeout(TimeSpan.FromMinutes(1))
            .Build();
    }

    protected override ManagedMqttClientOptions BuildManagedMqttClientOptions(ManagedMqttClientOptionsBuilder managedMqttClientOptionsBuilder)
    {
        return managedMqttClientOptionsBuilder
            .WithMaxPendingMessages(1000)
            .WithPendingMessagesOverflowStrategy(MqttPendingMessagesOverflowStrategy.DropOldestQueuedMessage)
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(15))
            .Build();
    }
}