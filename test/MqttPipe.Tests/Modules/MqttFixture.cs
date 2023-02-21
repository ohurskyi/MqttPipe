using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MqttPipe.Clients;
using MqttPipe.Configuration.DependencyInjection;
using MqttPipe.Services;
using MqttPipe.Tests.Handlers;
using MqttPipe.Tests.Options;
using Xunit;

namespace MqttPipe.Tests.Modules;

public class MqttFixture : IAsyncLifetime
{
    private const int Port = 5000;
    private readonly MqttTestConfiguration _testConfiguration = new(Port);
    private readonly MqttTestContainer _testContainer;

    public MqttFixture()
    {
        _testContainer = new TestcontainersBuilder<MqttTestContainer>()
            .WithMessageBroker(_testConfiguration)
            .Build();
    }

    public IMessageBus<TestMessagingClientOptions> MessageBus => ServiceProvider.GetRequiredService<IMessageBus<TestMessagingClientOptions>>();

    public IServiceProvider ServiceProvider { get; private set; }

    public async Task InitializeAsync()
    {
        await _testContainer.StartAsync().ConfigureAwait(false);
        ServiceProvider = BuildContainer();
        ServiceProvider.UseMqttMessageReceivedHandler<TestMessagingClientOptions>();
        var messagingHostedService = ServiceProvider.GetRequiredService<MqttMessagingHostedService<TestMessagingClientOptions>>();
        var mqttMessagingClient = ServiceProvider.GetRequiredService<IMqttMessagingClient<TestMessagingClientOptions>>();
        await messagingHostedService.StartAsync(CancellationToken.None);
        await mqttMessagingClient.Connect();
    }

    public async Task DisposeAsync()
    {
        var messagingHostedService = ServiceProvider.GetRequiredService<MqttMessagingHostedService<TestMessagingClientOptions>>();
        await messagingHostedService.StopAsync(CancellationToken.None);
        _testConfiguration.Dispose();
        await _testContainer.DisposeAsync();
    }

    private IServiceProvider BuildContainer()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<TextWriter>(p => new StringWriter(new StringBuilder()));
        serviceCollection.AddSingleton<ILoggerFactory, NullLoggerFactory>();
        serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        
        serviceCollection.AddMqttPipe<TestMessagingClientOptions, TestClientOptionsBuilder>(options =>
        {
            options.MqttBrokerConnectionOptions.Host = _testContainer.Hostname;
            options.MqttBrokerConnectionOptions.Port = _testContainer.Port;
        });
        serviceCollection.AddMessageHandlers(typeof(HandlerForDeviceNumber1).Assembly);
        return serviceCollection.BuildServiceProvider();
    }
}