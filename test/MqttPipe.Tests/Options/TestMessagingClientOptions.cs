using System;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Tests.Options;

public class TestMessagingClientOptions : IMqttMessagingClientOptions
{
    public MqttBrokerConnectionOptions MqttBrokerConnectionOptions { get; set; } = new();
}

public class TesClientOptionsBuilder : ClientOptionsBuilder<TestMessagingClientOptions>
{
    public TesClientOptionsBuilder(TestMessagingClientOptions clientOptions) : base(clientOptions)
    {
    }

    protected override MqttClientOptions BuildClientOptions(MqttClientOptionsBuilder mqttClientOptionsBuilder)
    {
        return mqttClientOptionsBuilder
            .WithClientId($"test-local-{Guid.NewGuid()}")
            .WithCleanSession()
            .Build();
    }

    protected override ManagedMqttClientOptions BuildManagedMqttClientOptions(ManagedMqttClientOptionsBuilder managedMqttClientOptionsBuilder)
    {
        return managedMqttClientOptionsBuilder
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
            .Build();
    }
}