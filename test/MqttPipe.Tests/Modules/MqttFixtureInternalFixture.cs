using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using MqttPipe.Tests.Modules;
using Xunit;

namespace MqttPipe.Tests;

public class MqttFixtureInternalFixture : IAsyncLifetime
{
    private const int Port = 5000;
    private readonly MqttTestConfiguration _testConfiguration = new(Port);
    private readonly MqttTestContainer _testContainer;
    private readonly IMqttClient _mqttClient;
    private MqttClientOptions _mqttClientOptions;

    public MqttFixtureInternalFixture()
    {
        _testContainer = new TestcontainersBuilder<MqttTestContainer>()
            .WithMessageBroker(_testConfiguration)
            .Build();

        _mqttClient = new MqttFactory().CreateMqttClient();
    }

    public IMqttClient MqttClient => _mqttClient;
    
    public async Task Connect() => await _mqttClient.ConnectAsync(_mqttClientOptions);

    public async Task InitializeAsync()
    {
        await _testContainer.StartAsync().ConfigureAwait(false);
        
        _mqttClientOptions = new MqttClientOptionsBuilder()
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .WithTcpServer(_testContainer.Hostname, _testContainer.Port)
            .WithCleanSession()
            .WithClientId($"test_client_{Guid.NewGuid()}")
            .Build();
    }

    public async Task DisposeAsync()
    {
        _testConfiguration.Dispose();
        await _testContainer.DisposeAsync();
        _mqttClient.Dispose();
    }
}