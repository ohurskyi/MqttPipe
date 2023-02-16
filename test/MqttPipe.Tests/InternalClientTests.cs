using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace MqttPipe.Tests;

public class InternalClientTests : IClassFixture<MqttFixtureInternalFixture>
{
    private readonly MqttFixtureInternalFixture _mqttFixture;
    private readonly ITestOutputHelper _outputHelper;

    public InternalClientTests(MqttFixtureInternalFixture mqttFixture, ITestOutputHelper outputHelper)
    {
        _mqttFixture = mqttFixture;
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Connected()
    {
        // Arrange

        var client = _mqttFixture.MqttClient;
        
        client.ConnectedAsync += args =>
        {
            _outputHelper.WriteLine("Connected");
            return Task.CompletedTask;
        };

        await _mqttFixture.Connect();
        
        TaskCompletionSource messageHandledCompletion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        client.ApplicationMessageReceivedAsync += async args =>
        {
            var payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
            _outputHelper.WriteLine("Received msg result {0}", payload);
            await Task.CompletedTask;
            messageHandledCompletion.SetResult();
        };
        
        const string topic = "test";
        await client.SubscribeAsync(new MqttClientSubscribeOptions
        {
            TopicFilters = new List<MqttTopicFilter> { new() { Topic = topic } }
        });
        
        var mqttApplicationMessage = new MqttApplicationMessageBuilder()
            .WithPayload("hello")
            .WithTopic(topic)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        // Act
        _outputHelper.WriteLine("Publishing msg");
        await client.PublishAsync(mqttApplicationMessage);

        _outputHelper.WriteLine("Waiting ...");
        await messageHandledCompletion.Task;
        
        // Assert
        Assert.True(client.IsConnected);
    }
}