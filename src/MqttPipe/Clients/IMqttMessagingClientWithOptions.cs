using MQTTnet;
using MQTTnet.Client;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Clients;

public interface IMqttMessagingClient<in TMessagingClientOptions> where TMessagingClientOptions : IMqttMessagingClientOptions
{
    Task StartAsync();

    Task StopAsync();

    Task SubscribeAsync(string topic);

    Task SubscribeAsync(IEnumerable<string> topics);
        
    Task UnsubscribeAsync(string topic);
        
    Task UnsubscribeAsync(IEnumerable<string> topics);

    Task PublishAsync(MqttApplicationMessage mqttApplicationMessage);

    void UseMqttMessageReceivedHandler(Func<MqttApplicationMessageReceivedEventArgs, Task> handler);
}