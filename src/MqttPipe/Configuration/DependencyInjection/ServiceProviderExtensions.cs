using Microsoft.Extensions.DependencyInjection;
using MqttPipe.Clients;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Configuration.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static void UseMqttMessageReceivedHandler<TMessagingClientOptions>(this IServiceProvider serviceProvider)
        where TMessagingClientOptions : class, IMqttMessagingClientOptions
    {
        var mqttMessagingClient = serviceProvider.GetRequiredService<IMqttMessagingClient<TMessagingClientOptions>>();
        var handler = serviceProvider.GetRequiredService<MqttReceivedMessageHandler<TMessagingClientOptions>>();
        mqttMessagingClient.UseMqttMessageReceivedHandler(handler.HandleApplicationMessageReceivedAsync);
    }
}