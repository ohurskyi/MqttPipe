using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration.DependencyInjection;
using MessagingLibrary.Core.Definitions.Subscriptions;
using MessagingLibrary.Core.Extensions;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Processing.Configuration.DependencyInjection;
using MqttPipe.Configuration.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MQTTnet.Client;
using MqttPipe.Extensions;
using MqttPipe.Tests.Clients;
using MqttPipe.Tests.Contracts;
using MqttPipe.Tests.Handlers;
using MqttPipe.Tests.Options;
using MqttPipe.Tests.Topics;
using Xunit;

namespace MqttPipe.Tests;

public class MqttReceivedMessageHandlerTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(420)]
    public async Task ExecuteAsync_MultiWildCardDeviceTopic_CallHandlerForAllDevices(int deviceNumber)
    {
        // arrange
        var builder = new StringBuilder();
        await using var writer = new StringWriter(builder);
        var serviceProvider = BuildContainer(writer);
        
        const string multiWildCardDeviceTopic = $"{DeviceTopicConstants.DeviceTopic}/#";
        var topicClient = serviceProvider.GetRequiredService<ITopicClient<TestMessagingClientOptions>>();
        await topicClient.Subscribe(new SubscriptionDefinition<HandlerForAllDeviceNumbers>(multiWildCardDeviceTopic));
        
        var contract =  new DeviceMessageContract { Name = "Device" };
        var publishTopic = $"{DeviceTopicConstants.DeviceTopic}/{deviceNumber}";
        var message = new Message { Topic = publishTopic, Payload = contract.MessagePayloadToJson() };

        // act
        var messageBus = serviceProvider.GetRequiredService<IMessageBus<TestMessagingClientOptions>>();
        await messageBus.Publish(message);
        var messageReceivedHandler = serviceProvider.GetRequiredService<InMemoryMessageReceivedHandler<TestMessagingClientOptions>>();
        await messageReceivedHandler.HandleApplicationMessageReceivedAsync();
        var result = builder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        // assert
        Assert.Contains("Device " + nameof(HandlerForAllDeviceNumbers), result);
    }
    
    [Fact]
    public async Task ExecuteAsync_ForDeviceNumberOneTopic_CallsHandlerForDeviceNumber1()
    {
        // arrange
        var builder = new StringBuilder();
        await using var writer = new StringWriter(builder);
        var serviceProvider = BuildContainer(writer);
        
        const int deviceNumberOne = 1;
        const int deviceNumberTwo = 2;

        var topicClient = serviceProvider.GetRequiredService<ITopicClient<TestMessagingClientOptions>>();
        await topicClient.Subscribe(new SubscriptionDefinition<HandlerForDeviceNumber1>($"{DeviceTopicConstants.DeviceTopic}/{deviceNumberOne}"));
        await topicClient.Subscribe(new SubscriptionDefinition<HandlerForDeviceNumber2>($"{DeviceTopicConstants.DeviceTopic}/{deviceNumberTwo}"));

        var contract =  new DeviceMessageContract { Name = "Device" };
        var publishTopic = $"{DeviceTopicConstants.DeviceTopic}/{deviceNumberOne}";
        var message = new Message { Topic = publishTopic, Payload = contract.MessagePayloadToJson() };

        // act
        var messageBus = serviceProvider.GetRequiredService<IMessageBus<TestMessagingClientOptions>>();
        await messageBus.Publish(message);
        var messageReceivedHandler = serviceProvider.GetRequiredService<InMemoryMessageReceivedHandler<TestMessagingClientOptions>>();
        await messageReceivedHandler.HandleApplicationMessageReceivedAsync();
        var result = builder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        // assert
        Assert.Contains("Device " + nameof(HandlerForDeviceNumber1), result);
        Assert.DoesNotContain("Device " + nameof(HandlerForDeviceNumber2), result);
    }
    
    [Fact]
    public async Task ExecuteAsync_SingleLevelWildCardDeviceTopic_CallsHandlerForAllDeviceNumbers()
    {
        // arrange
        var builder = new StringBuilder();
        await using var writer = new StringWriter(builder);
        var serviceProvider = BuildContainer(writer);

        const int deviceNumberOne = 1;
        const int deviceNumberTwo = 2;

        var topicClient = serviceProvider.GetRequiredService<ITopicClient<TestMessagingClientOptions>>();
        await topicClient.Subscribe(new SubscriptionDefinition<HandlerForDeviceNumber1>($"{DeviceTopicConstants.DeviceTopic}/+/temperature/{deviceNumberOne}"));
        await topicClient.Subscribe(new SubscriptionDefinition<HandlerForDeviceNumber2>($"{DeviceTopicConstants.DeviceTopic}/+/temperature/{deviceNumberTwo}"));
        await topicClient.Subscribe(new SubscriptionDefinition<HandlerForAllDeviceNumbers>($"{DeviceTopicConstants.DeviceTopic}/+/temperature"));

        var contract =  new DeviceMessageContract { Name = "Device" };
        var publishTopic = $"{DeviceTopicConstants.DeviceTopic}/{deviceNumberOne}/temperature";
        var message = new Message { Topic = publishTopic, Payload = contract.MessagePayloadToJson() };
        
        // act
        var messageBus = serviceProvider.GetRequiredService<IMessageBus<TestMessagingClientOptions>>();
        await messageBus.Publish(message);
        var messageReceivedHandler = serviceProvider.GetRequiredService<InMemoryMessageReceivedHandler<TestMessagingClientOptions>>();
        await messageReceivedHandler.HandleApplicationMessageReceivedAsync();
        var result = builder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        // assert
        Assert.Contains("Device " + nameof(HandlerForAllDeviceNumbers), result);
        Assert.DoesNotContain("Device " + nameof(HandlerForDeviceNumber1), result);
        Assert.DoesNotContain("Device " + nameof(HandlerForDeviceNumber2), result);
    }
    
    
    private static IServiceProvider BuildContainer(TextWriter textWriter)
    {
        var serviceCollection = new ServiceCollection();
        
        serviceCollection.AddSingleton(textWriter);
        serviceCollection.AddSingleton<ILoggerFactory, NullLoggerFactory>();
        serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        
        serviceCollection.AddSingleton<IMessageBus<TestMessagingClientOptions>, InMemoryMessageBus<TestMessagingClientOptions>>();
        serviceCollection.AddSingleton<InMemoryMessageChannel>();
        serviceCollection.AddSingleton<InMemoryMessageReceivedHandler<TestMessagingClientOptions>>();
        
        serviceCollection.AddSingleton<ITopicClient<TestMessagingClientOptions>, InMemoryTopicClient<TestMessagingClientOptions>>();
        serviceCollection.AddMessagingPipeline<TestMessagingClientOptions>();
        serviceCollection.AddMqttTopicComparer();
        serviceCollection.AddMessageHandlers(typeof(HandlerForDeviceNumber1).Assembly);
        
        return serviceCollection.BuildServiceProvider();
    }
}