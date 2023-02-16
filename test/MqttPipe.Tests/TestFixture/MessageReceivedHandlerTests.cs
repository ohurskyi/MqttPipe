using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MqttPipe.Configuration.DependencyInjection;
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

    [Fact]
    public async Task Received()
    {
        // Arrange
        var provider = _fixture.ServiceProvider;

        Task ConnectedFunc()
        {
            _outputHelper.WriteLine("Connected");
            return Task.CompletedTask;
        }

        await _fixture.Connect(ConnectedFunc);
        
        provider.UseMqttMessageReceivedHandler<TestMessagingClientOptions>();

        var contract =  new DeviceMessageContract { Name = "Device" };
        var deviceNumber = 1;
        var publishTopic = $"{DeviceTopicConstants.DeviceTopic}/{deviceNumber}";

        // Act
        _outputHelper.WriteLine("Publishing msg");
        var client = _fixture.MqttClient;
        await client.Publish(contract, publishTopic);

        _outputHelper.WriteLine("Waiting for message to be processed...");
        await Task.Delay(TimeSpan.FromSeconds(5));

        var textWriter = (StringWriter)provider.GetRequiredService<TextWriter>();
        var result = textWriter.GetStringBuilder().ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        
        // Assert
        Assert.Contains("Device " + nameof(HandlerForAllDeviceNumbers), result);
    }
}