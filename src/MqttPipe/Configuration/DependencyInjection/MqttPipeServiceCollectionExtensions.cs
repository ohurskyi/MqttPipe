using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Configuration.DependencyInjection;

public static class MqttPipeServiceCollectionExtensions
{
    public static IServiceCollection AddMqttPipe<TMessagingClientOptions, TClientOptionsBuilder>(this IServiceCollection serviceCollection, IConfiguration configuration)
        where TMessagingClientOptions : class, IMqttMessagingClientOptions, new()
        where TClientOptionsBuilder: class, IClientOptionsBuilder<TMessagingClientOptions>
    {
        serviceCollection.AddMqttMessagingClient<TMessagingClientOptions, TClientOptionsBuilder>(configuration);
        serviceCollection.AddMqttMessageBus<TMessagingClientOptions>();
        serviceCollection.AddMqttTopicClient<TMessagingClientOptions>();
        serviceCollection.AddMqttRequestClient<TMessagingClientOptions>();
        serviceCollection.AddMqttMessageProcessing<TMessagingClientOptions>();
        return serviceCollection;
    }
}