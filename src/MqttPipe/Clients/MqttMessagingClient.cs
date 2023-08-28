using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Clients;

public class MqttMessagingClient<TMessagingClientOptions> : IMqttMessagingClient<TMessagingClientOptions>, IDisposable
    where TMessagingClientOptions : IMqttMessagingClientOptions
{
    private readonly IManagedMqttClient _mqttClient;
    private readonly ManagedMqttClientOptions _mqttClientOptions;

    public MqttMessagingClient(IClientOptionsBuilder<TMessagingClientOptions> clientOptionsBuilder)
    {
        _mqttClientOptions = clientOptionsBuilder.BuildManagedMqttClientOptions();

        _mqttClient = new MqttFactory().CreateManagedMqttClient();
    }

    public void UseMqttMessageReceivedHandler(Func<MqttApplicationMessageReceivedEventArgs, Task> handler)
    {
        _mqttClient.ApplicationMessageReceivedAsync += handler;
    }

    public async Task StartAsync()
    {
        await _mqttClient.StartAsync(_mqttClientOptions);
    }

    public Task Connect()
    {
        TaskCompletionSource tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

        _mqttClient.ConnectedAsync += Func;
        return tcs.Task;

        Task Func(MqttClientConnectedEventArgs args)
        {
            _mqttClient.ConnectedAsync -= Func;
            tcs.TrySetResult();
            return Task.CompletedTask;
        }
    }

    public async Task StopAsync()
    {
        await _mqttClient.StopAsync();
    }
        
    public Task SubscribeAsync(string topic)
    {
        return _mqttClient.SubscribeAsync(topic);
    }

    public Task SubscribeAsync(IEnumerable<string> topics)
    {
        return _mqttClient.SubscribeAsync(topics.Select(t => new MqttTopicFilterBuilder().WithTopic(t).Build()).ToList());
    }
        
    public Task UnsubscribeAsync(string topic)
    {
        return _mqttClient.UnsubscribeAsync(topic);
    }

    public Task UnsubscribeAsync(IEnumerable<string> topics)
    {
        return _mqttClient.UnsubscribeAsync(topics.ToList());
    }

    public async Task PublishAsync(MqttApplicationMessage mqttApplicationMessage)
    {
        await _mqttClient.EnqueueAsync(mqttApplicationMessage);
    }

    public void Dispose()
    {
        _mqttClient?.Dispose();
    }
}