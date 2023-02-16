using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration.DependencyInjection;
using MessagingLibrary.Processing.Configuration.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MqttPipe.Clients;
using MqttPipe.Configuration.DependencyInjection;
using MqttPipe.Tests.Consumers;
using MqttPipe.Tests.Handlers;
using MqttPipe.Tests.Modules;
using MqttPipe.Tests.Options;
using Xunit;

namespace MqttPipe.Tests.TestFixture;

public class MqttMessageHandlingFixture : IAsyncLifetime
{
    private const int Port = 5000;
    private readonly MqttTestConfiguration _testConfiguration = new(Port);
    private readonly MqttTestContainer _testContainer;
    private readonly IManagedMqttClient _managedMqttClient;

    private TextWriter _writer;
    private IServiceProvider _serviceProvider;

    public MqttMessageHandlingFixture()
    {
        _testContainer = new TestcontainersBuilder<MqttTestContainer>()
            .WithMessageBroker(_testConfiguration)
            .Build();
    }

    public IMessageBus<TestMessagingClientOptions> MqttClient =>  _serviceProvider.GetRequiredService<IMessageBus<TestMessagingClientOptions>>();

    public IServiceProvider ServiceProvider => _serviceProvider;

    public async Task Connect(Func<Task> connectedHandler)
    {
        var hostedServices = _serviceProvider.GetServices<IHostedService>();
        foreach (var hostedService in hostedServices)
        {
            await hostedService.StartAsync(CancellationToken.None);
        }

        var mqttMessagingClient = _serviceProvider.GetRequiredService<IMqttMessagingClient<TestMessagingClientOptions>>();
        await mqttMessagingClient.WaitConnected(connectedHandler);
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

        new ManagedMqttClientOptionsBuilder()
            .WithClientOptions(mqttClientOptions)
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
            .Build();

        _serviceProvider = BuildContainer();
    }

    public async Task DisposeAsync()
    {
        _testConfiguration.Dispose();
        await _testContainer.DisposeAsync();
    }

    private IServiceProvider BuildContainer()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<TextWriter>(p => new StringWriter(new StringBuilder()));
        serviceCollection.AddSingleton<ILoggerFactory, NullLoggerFactory>();
        serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        
        serviceCollection.AddMqttPipe<TestMessagingClientOptions, TesClientOptionsBuilder>(options =>
        {
            // null
            options.MqttBrokerConnectionOptions.Host = _testContainer.Hostname;
            options.MqttBrokerConnectionOptions.Port = _testContainer.Port;
        });
        serviceCollection.AddMessageHandlers(typeof(HandlerForDeviceNumber1).Assembly);
        serviceCollection.AddConsumerDefinitionListenerProvider<ConsumerDefinitionListenerProvider>();
        serviceCollection.AddMessageConsumersHostedService();
        
        return serviceCollection.BuildServiceProvider();
    }
}