# MqttPipe
Simple MQTT message processing library

Message processing is done using [MessagingLibrary Core](https://github.com/ohurskyi/MQTT/tree/main/src/MessagingLibrary.Core)/[Processing](https://github.com/ohurskyi/MQTT/tree/main/src/MessagingLibrary.Processing) with integration of [MQTTnet](https://github.com/chkr1011/MQTTnet).

Core/Processing has no dependencies. Integration with MQTT done in separate proj [MqttPipe](https://github.com/ohurskyi/MQTT/tree/main/src/MqttPipe).

## Define messaging options
each client should have its own implementation of ```IMqttMessagingClientOptions```
```csharp
public class InfrastructureClientOptions : IMqttMessagingClientOptions
{
    public MqttBrokerConnectionOptions MqttBrokerConnectionOptions { get; set; } = new() { Host = "infrastructure.dev.com", Port = 1883 };
}
```
and derive from ```ClientOptionsBuilder<T>```, in our case ```T is InfrastructureClientOptions```
```csharp
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
```

## Create Message Contract
```csharp
public class DeviceMessageContract : IMessageContract
{
    public string Name { get; set; }
}
```

## Create handler
```csharp
public class DeviceHandler : MessageHandlerBase<DeviceMessageContract>
{
    private readonly TextWriter _textWriter;

    public DeviceHandler(TextWriter textWriter)
    {
        _textWriter = textWriter;
    }
    
    protected override async Task<IExecutionResult> HandleAsync(MessagingContext<DeviceMessageContract> messagingContext)
    {
        DeviceMessageContract payload = messagingContext.Payload;
        await _textWriter.WriteLineAsync("Received device with name: " + payload.Name);
        return new SuccessfulResult();
    }
}
```

## Consumers
implement ```IConsumerDefinition``` for mapping handler(s) to a topic
```csharp
public class DeviceConsumerDefinition : IConsumerDefinition
{
    private readonly List<int> _availableDeviceNumbers;

    public DeviceConsumerDefinition(List<int> availableDeviceNumbers)
    {
        _availableDeviceNumbers = availableDeviceNumbers;
    }

    public IEnumerable<ISubscriptionDefinition> Definitions()
    {
        return _availableDeviceNumbers.Select(deviceNumber => new SubscriptionDefinition<DeviceHandler>($"device/{deviceNumber}"));
    }
}
```
aggregate one/multiple definitions in a provider
```csharp
public class ConsumerDefinitionProvider : IConsumerDefinitionProvider
{
    private readonly IAvailableDevicesConfig _availableDevicesConfig;

    public ConsumerDefinitionProvider(IAvailableDevicesConfig availableDevicesConfig)
    {
        _availableDevicesConfig = availableDevicesConfig;
    }

    public IEnumerable<IConsumerDefinition> Definitions => new List<IConsumerDefinition>
    {
        new DeviceConsumerDefinition(_availableDevicesConfig.GetDevices())
    };
}
```
create listener, then subscriptions will be created on startup and removed on tear down of the host.
```csharp
public class ConsumerDefinitionListenerProvider : IConsumerDefinitionListenerProvider
{
    private readonly ITopicClient<InfrastructureClientOptions> _topicClient;

    public ConsumerDefinitionListenerProvider(ITopicClient<InfrastructureClientOptions> topicClient)
    {
        _topicClient = topicClient;
    }

    public IEnumerable<IConsumerListener> Listeners => new List<IConsumerListener>
    {
        new ConsumerListener<InfrastructureClientOptions>(_topicClient, new ConsumerDefinitionProvider())
    };
}
```
if you want dynamically map the handler to the topic
```csharp
private readonly ITopicClient<InfrastructureClientOptions> _topicClient;
...

if (configurationSource == "external")
{
    var deviceTopicName = $"devices/remote/{remoteDeviceNumber}";
    _topicClient.Subscribe(new SubscriptionDefinition<DeviceHandler>(deviceTopicName));
}
...
```

## Register in DI
```csharp
public static IServiceCollection AddDeviceDomainServices(this IServiceCollection serviceCollection)
{
    serviceCollection.AddMessageHandlers(typeof(DeviceHandler).Assembly);
    serviceCollection.AddConsumerDefinitionListenerProvider<ConsumerDefinitionListenerProvider>();
    serviceCollection.AddMessageConsumersHostedService();
}
```
## Append message processing for a specific client
```csharp
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, serviceCollection) =>
        {
            var configuration = hostContext.Configuration;
           
            serviceCollection.AddDeviceDomainServices();
            
            serviceCollection.AddMqttPipe<InfrastructureClientOptions, InfrastructureClientOptionsBuilder>(configuration);
        })
...

host.Services.UseMqttMessageReceivedHandler<InfrastructureClientOptions>();
```

To get started please check the samples: (containing multiple handlers and integration events): 
1. [Distributed config](https://github.com/ohurskyi/MQTT/tree/main/samples/distributedconfiguration) 
2. [Shooting](https://github.com/ohurskyi/MQTT/tree/main/samples/drawing)
