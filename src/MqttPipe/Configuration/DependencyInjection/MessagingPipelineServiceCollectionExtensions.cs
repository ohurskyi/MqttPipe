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
        return serviceCollection
            .AddMessagingPipeline<TMessagingClientOptions>()
            .AddMiddlewares()
            .AddMqttTopicComparer()
            .AddMqttApplicationMessageReceivedHandler<TMessagingClientOptions>();
    }

    private static IServiceCollection AddMiddlewares(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddEnumerable(new[]
        {
            ServiceDescriptor.Transient(typeof(IMessageMiddleware<,>), typeof(UnhandledExceptionMiddleware<,>)),
            ServiceDescriptor.Transient(typeof(IMessageMiddleware<,>), typeof(LoggingMiddleware<,>)),
            ServiceDescriptor.Transient(typeof(IMessageMiddleware<,>), typeof(PerformanceMiddleware<,>)),
            ServiceDescriptor.Transient(typeof(IMessageMiddleware<,>), typeof(PublishMiddleware<,>)),
            ServiceDescriptor.Transient(typeof(IMessageMiddleware<,>), typeof(ReplyMiddleware<,>)),
        });

        return serviceCollection;
    }
}