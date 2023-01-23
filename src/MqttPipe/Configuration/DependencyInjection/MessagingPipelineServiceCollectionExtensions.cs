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
            .AddMqttApplicationMessageReceivedHandler<TMessagingClientOptions>();
    }

    public static IServiceCollection AddMiddlewaresNew(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddEnumerable(new[]
        {
            ServiceDescriptor.Transient(typeof(IMessageMiddleware<>),
                typeof(UnhandledExceptionMiddleware<>)),
            ServiceDescriptor.Transient(typeof(IMessageMiddleware<>), typeof(LoggingMiddleware<>)),
            ServiceDescriptor.Transient(typeof(IMessageMiddleware<>), typeof(PublishMiddleware<>)),
            ServiceDescriptor.Transient(typeof(IMessageMiddleware<>), typeof(ReplyMiddleware<>)),
        });
        
        return serviceCollection;
    }
}