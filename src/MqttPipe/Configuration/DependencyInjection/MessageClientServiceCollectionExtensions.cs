﻿using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MqttPipe.Clients;
using MqttPipe.Clients.RequestResponse;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Configuration.DependencyInjection;

public static class MessageClientServiceCollectionExtensions
{
    public static IServiceCollection AddMqttMessageBus<TMessagingClientOptions>(this IServiceCollection serviceCollection) where TMessagingClientOptions: class, IMqttMessagingClientOptions
    {
        serviceCollection.TryAddSingleton<IMessageBus<TMessagingClientOptions>, MqttMessageBus<TMessagingClientOptions>>();
        return serviceCollection;
    }
    
    public static IServiceCollection AddMqttTopicClient<TMessagingClientOptions>(this IServiceCollection serviceCollection) 
        where TMessagingClientOptions: class, IMqttMessagingClientOptions
    {
        serviceCollection.TryAddSingleton<ITopicClient<TMessagingClientOptions>, MqttTopicClient<TMessagingClientOptions>>();
        return serviceCollection;
    }
    
    public static IServiceCollection AddMqttRequestClient<TMessagingClientOptions>(this IServiceCollection serviceCollection) 
        where TMessagingClientOptions: class, IMqttMessagingClientOptions
    {
        serviceCollection.TryAddSingleton(typeof(PendingResponseTracker<>));
        serviceCollection.TryAddTransient(typeof(Requester<,,>));
        serviceCollection.TryAddTransient(typeof(ResponseHandler<>));
        serviceCollection.TryAddSingleton<IRequestClient<TMessagingClientOptions>, RequestClient<TMessagingClientOptions>>();
        return serviceCollection;
    }
}