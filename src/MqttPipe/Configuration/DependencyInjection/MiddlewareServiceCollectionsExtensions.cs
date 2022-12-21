using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Configuration.DependencyInjection;

public static class MiddlewareServiceCollectionsExtensions
{
    public static IServiceCollection AddMiddleware<TMiddleware, TMessagingClientOptions>(this IServiceCollection serviceCollection) 
        where TMessagingClientOptions : class, IMqttMessagingClientOptions
        where TMiddleware : class, IMessageMiddleware<TMessagingClientOptions>
    {
        serviceCollection.TryAddEnumerable(new [] {ServiceDescriptor.Transient<IMessageMiddleware<TMessagingClientOptions>, TMiddleware>() });
        return serviceCollection;
    }
}