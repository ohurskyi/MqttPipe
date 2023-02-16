using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Packets;
using MQTTnet.Protocol;
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
    public async Task Connected()
    {
        // Arrange
        var provider = _fixture.ServiceProvider;

        Task ConnectedFunc()
        {
            _outputHelper.WriteLine("Connected");
            return Task.CompletedTask;
        }

        await _fixture.Connect(ConnectedFunc);
        
        TaskCompletionSource messageHandledCompletion = new(TaskCreationOptions.RunContinuationsAsynchronously);

        const string topic = "test";
        //await client.SubscribeAsync(topic);
        
        var mqttApplicationMessage = new MqttApplicationMessageBuilder()
            .WithPayload("hello")
            .WithTopic(topic)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
            .Build();

        // Act
        _outputHelper.WriteLine("Publishing msg");
        var client = _fixture.MqttClient;
        await client.PublishAsync(mqttApplicationMessage);

        _outputHelper.WriteLine("Waiting for message to be processed...");
        await messageHandledCompletion.Task.ConfigureAwait(false);

        var textWriter = (StringWriter)provider.GetRequiredService<TextWriter>();
        var result = textWriter.GetStringBuilder().ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        
        // Assert
        Assert.NotEmpty(result);
    }
}