using System;
using System.IO;
using System.Threading.Tasks;
using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Definitions.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using MqttPipe.Tests.Contracts;
using MqttPipe.Tests.Handlers;
using MqttPipe.Tests.Options;
using MqttPipe.Tests.Topics;
using Xunit;
using Xunit.Abstractions;

namespace MqttPipe.Tests.TestFixture;

public class MessageReceivedHandlerTests : IClassFixture<MqttMessageHandlingFixture>
{
    private readonly MqttMessageHandlingFixture _fixture;
    private readonly ITestOutputHelper _outputHelper;

    public MessageReceivedHandlerTests(MqttMessageHandlingFixture fixture, ITestOutputHelper outputHelper)
    {
        _fixture = fixture;
        _outputHelper = outputHelper;
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
}