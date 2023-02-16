using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using Xunit;

namespace MqttPipe.Tests.Modules;

public class MqttFixtureManagedClient : IAsyncLifetime
{
    private const int Port = 5000;
    private readonly MqttTestConfiguration _testConfiguration = new(Port);
    private readonly MqttTestContainer _testContainer;
    private readonly IManagedMqttClient _managedMqttClient;
    private ManagedMqttClientOptions _clientOptions;

    private readonly TaskCompletionSource _taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public MqttFixtureManagedClient()
    {
        _testContainer = new TestcontainersBuilder<MqttTestContainer>()
            .WithMessageBroker(_testConfiguration)
            .Build();

        _managedMqttClient = new MqttFactory().CreateManagedMqttClient();
    }

    public IManagedMqttClient MqttClient => _managedMqttClient;

    public async Task Connect(Func<Task> connectedHandler)
    {
        _managedMqttClient.ConnectedAsync += async args =>
        {
            await connectedHandler();
            _taskCompletionSource.SetResult();
        };
        
        await _managedMqttClient.StartAsync(_clientOptions);
        await _taskCompletionSource.Task;
    }

    public async Task InitializeAsync()
    {
        await _testContainer.StartAsync().ConfigureAwait(false);
        
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .WithTcpServer(_testContainer.Hostname, _testContainer.Port)
            .WithCleanSession()
            .WithClientId($"den_client_{Guid.NewGuid()}")
            .Build();

        _clientOptions = new ManagedMqttClientOptionsBuilder()
            .WithClientOptions(mqttClientOptions)
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
            .Build();
    }

    public async Task DisposeAsync()
    {
        _testConfiguration.Dispose();
        await _testContainer.DisposeAsync();
        _managedMqttClient.Dispose();
    }
}