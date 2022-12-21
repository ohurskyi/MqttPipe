using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Definitions.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using MqttPipe.Tests.Contracts;
using MqttPipe.Tests.Handlers;
using MqttPipe.Tests.Modules;
using MqttPipe.Tests.Options;
using MqttPipe.Tests.Topics;
using Xunit;

namespace MqttPipe.Tests;

public class MessageReceivedHandlerTests : IClassFixture<MqttFixture>
{
    private readonly MqttFixture _fixture;

    public MessageReceivedHandlerTests(MqttFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(420)]
    public async Task ExecuteAsync_MultiWildCardDeviceTopic_CallHandlerForAllDevices(int deviceNumber)
    {
        // Arrange
        var provider = _fixture.ServiceProvider;
        
        const string multiWildCardDeviceTopic = $"{DeviceTopicConstants.DeviceTopic}/#";
        var topicClient = provider.GetRequiredService<ITopicClient<TestMessagingClientOptions>>();
        await topicClient.Subscribe(new SubscriptionDefinition<HandlerForAllDeviceNumbers>(multiWildCardDeviceTopic));

        var contract = new DeviceMessageContract { Name = "Device" };
        var publishTopic = $"{DeviceTopicConstants.DeviceTopic}/{deviceNumber}";

        // Act
        var messageBus = _fixture.MessageBus;
        await messageBus.Publish(contract, publishTopic);
        await Task.Delay(TimeSpan.FromSeconds(5));
        var textWriter = (StringWriter)provider.GetRequiredService<TextWriter>();
        var result = textWriter.GetStringBuilder().ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        
        // Assert
        Assert.Contains("Device " + nameof(HandlerForAllDeviceNumbers), result);
    }
    
    [Fact]
    public async Task ExecuteAsync_ForDeviceNumberOneTopic_CallsHandlerForDeviceNumber1()
    {
        // arrange
        var provider = _fixture.ServiceProvider;
        
        const int deviceNumberOne = 1;
        const int deviceNumberTwo = 2;

        var topicClient = provider.GetRequiredService<ITopicClient<TestMessagingClientOptions>>();
        await topicClient.Subscribe(new List<ISubscriptionDefinition>
        {
            new SubscriptionDefinition<HandlerForDeviceNumber1>($"{DeviceTopicConstants.DeviceTopic}/{deviceNumberOne}"),
            new SubscriptionDefinition<HandlerForDeviceNumber2>($"{DeviceTopicConstants.DeviceTopic}/{deviceNumberTwo}")
        });

        var contract = new DeviceMessageContract { Name = "Device" };
        var publishTopic = $"{DeviceTopicConstants.DeviceTopic}/{deviceNumberOne}";

        // act
        var messageBus = _fixture.MessageBus;
        await messageBus.Publish(contract, publishTopic);
        await Task.Delay(TimeSpan.FromSeconds(5));
        var textWriter = (StringWriter)provider.GetRequiredService<TextWriter>();
        var result = textWriter.GetStringBuilder().ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        // assert
        Assert.Contains("Device " + nameof(HandlerForDeviceNumber1), result);
        Assert.DoesNotContain("Device " + nameof(HandlerForDeviceNumber2), result);
    }
    
    [Fact]
    public async Task ExecuteAsync_SingleLevelWildCardDeviceTopic_CallsHandlerForAllDeviceNumbers()
    {
        // arrange
        var provider = _fixture.ServiceProvider;

        const int deviceNumberOne = 1;
        const int deviceNumberTwo = 2;

        var topicClient = provider.GetRequiredService<ITopicClient<TestMessagingClientOptions>>();
        await topicClient.Subscribe(new List<ISubscriptionDefinition>
        {
            new SubscriptionDefinition<HandlerForDeviceNumber1>($"{DeviceTopicConstants.DeviceTopic}/+/temperature/{deviceNumberOne}"),
            new SubscriptionDefinition<HandlerForDeviceNumber2>($"{DeviceTopicConstants.DeviceTopic}/+/temperature/{deviceNumberTwo}"),
            new SubscriptionDefinition<HandlerForAllDeviceNumbers>($"{DeviceTopicConstants.DeviceTopic}/+/temperature")
        });

        var contract = new DeviceMessageContract { Name = "Device" };
        var publishTopic = $"{DeviceTopicConstants.DeviceTopic}/room_1/temperature";

        // act
        var messageBus = _fixture.MessageBus;
        await messageBus.Publish(contract, publishTopic);
        await Task.Delay(TimeSpan.FromSeconds(5));
        var textWriter = (StringWriter)provider.GetRequiredService<TextWriter>();
        var result = textWriter.GetStringBuilder().ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        // assert
        Assert.Contains("Device " + nameof(HandlerForAllDeviceNumbers), result);
        Assert.DoesNotContain("Device " + nameof(HandlerForDeviceNumber1), result);
        Assert.DoesNotContain("Device " + nameof(HandlerForDeviceNumber2), result);
    }
}