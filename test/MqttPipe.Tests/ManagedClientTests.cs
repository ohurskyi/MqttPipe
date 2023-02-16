using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MqttPipe.Tests.Modules;
using Xunit;
using Xunit.Abstractions;

namespace MqttPipe.Tests;

public class ManagedClientTests : IClassFixture<MqttFixtureManagedClient>
{
    private readonly MqttFixtureManagedClient _mqttFixture;
    private readonly ITestOutputHelper _outputHelper;

    public ManagedClientTests(MqttFixtureManagedClient mqttFixture, ITestOutputHelper outputHelper)
    {
        _mqttFixture = mqttFixture;
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public async Task Connected()
    {
        // Arrange
        var client = _mqttFixture.MqttClient;

        Task ConnectedFunc()
        {
            _outputHelper.WriteLine("Connected");
            return Task.CompletedTask;
        }

        await _mqttFixture.Connect(ConnectedFunc);
        
        TaskCompletionSource messageHandledCompletion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        
        client.ApplicationMessageReceivedAsync += async args =>
        {
            var payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
            _outputHelper.WriteLine("Received msg result {0}", payload);
            await Task.CompletedTask;
            messageHandledCompletion.SetResult();
        };
        
        const string topic = "test";
        await client.SubscribeAsync(new List<MqttTopicFilter> { new() { Topic = topic } });
        
        var mqttApplicationMessage = new MqttApplicationMessageBuilder()
            .WithPayload("hello")
            .WithTopic(topic)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
            .Build();

        // Act
        _outputHelper.WriteLine("Publishing msg");
        await client.EnqueueAsync(mqttApplicationMessage);

        _outputHelper.WriteLine("Waiting for message to be processed...");
        await messageHandledCompletion.Task.ConfigureAwait(false);
        
        // Assert
        Assert.True(client.IsConnected);
    }
}