using MessagingLibrary.Processing.Configuration.DependencyInjection;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MqttPipe.Configuration.Configuration;
using MqttPipe.Middlewares;

namespace MqttPipe.Configuration.DependencyInjection;

public static class MessagingPipelineServiceCollectionExtensions
{
    public static IServiceCollection AddMqttMessageProcessing<TMessagingClientOptions>(this IServiceCollection serviceCollection)
        where TMessagingClientOptions : class, IMqttMessagingClientOptions
    {
        // test
        serviceCollection.AddMiddlewaresNew();
        
        return serviceCollection
            .AddMessagingPipeline<TMessagingClientOptions>()
            .AddMqttTopicComparer()
            .AddInternalMiddlewares<TMessagingClientOptions>()
            .AddMqttApplicationMessageReceivedHandler<TMessagingClientOptions>();
    }

    public static IServiceCollection AddMiddlewaresNew(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddEnumerable(new[]
        {
            ServiceDescriptor.Transient(typeof(IMessageMiddlewareGeneric<>),
                typeof(UnhandledExceptionMiddlewareGeneric<>)),
            ServiceDescriptor.Transient(typeof(IMessageMiddlewareGeneric<>), typeof(LoggingMiddlewareGeneric<>)),
            ServiceDescriptor.Transient(typeof(IMessageMiddlewareGeneric<>), typeof(PublishMiddlewareGeneric<>)),
            ServiceDescriptor.Transient(typeof(IMessageMiddlewareGeneric<>), typeof(ReplyMiddlewareGeneric<>)),
        });
        
        return serviceCollection;
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