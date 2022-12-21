using MessagingLibrary.Processing.Configuration.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MqttPipe.Configuration.Configuration;
using MqttPipe.Middlewares;

namespace MqttPipe.Configuration.DependencyInjection;

public static class MessagingPipelineServiceCollectionExtensions
{
    public static IServiceCollection AddMqttMessageProcessing<TMessagingClientOptions>(this IServiceCollection serviceCollection)
        where TMessagingClientOptions : class, IMqttMessagingClientOptions
    {
        return serviceCollection
            .AddMessagingPipeline<TMessagingClientOptions>()
            .AddMqttTopicComparer()
            .AddInternalMiddlewares<TMessagingClientOptions>()
            .AddMqttApplicationMessageReceivedHandler<TMessagingClientOptions>();
    }

    private static IServiceCollection AddInternalMiddlewares<TMessagingClientOptions>(this IServiceCollection serviceCollection)
        where TMessagingClientOptions : class, IMqttMessagingClientOptions
    {
        serviceCollection.AddMiddleware<UnhandledExceptionMiddleware<TMessagingClientOptions>, TMessagingClientOptions>();
        serviceCollection.AddMiddleware<LoggingMiddleware<TMessagingClientOptions>, TMessagingClientOptions>();
        serviceCollection.AddMiddleware<PublishMiddleware<TMessagingClientOptions>, TMessagingClientOptions>();
        serviceCollection.AddMiddleware<ReplyMiddleware<TMessagingClientOptions>, TMessagingClientOptions>();
        return serviceCollection;
    }
}