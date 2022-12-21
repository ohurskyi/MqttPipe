using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Configuration.DependencyInjection;
using MessagingLibrary.Processing.Executor;
using MessagingLibrary.Processing.Middlewares;
using MessagingLibrary.Processing.Strategy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MessagingLibrary.Processing.Configuration.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingPipeline<TMessagingClientOptions>(this IServiceCollection serviceCollection)
        where TMessagingClientOptions: class, IMessagingClientOptions
    {
        serviceCollection.AddRequiredServices<TMessagingClientOptions>();

        return serviceCollection;
    }

    private static IServiceCollection AddRequiredServices<TMessagingClientOptions>(this IServiceCollection serviceCollection) 
        where TMessagingClientOptions: class, IMessagingClientOptions
    {
        serviceCollection.AddMessageHandlerFactory<TMessagingClientOptions>();

        serviceCollection.AddMessageSerialization();
        
        serviceCollection.TryAddSingleton<IMessageExecutor<TMessagingClientOptions>, ScopedMessageExecutor<TMessagingClientOptions>>();
        
        serviceCollection.TryAddTransient(typeof(MessageHandlingStrategy<,>));

        serviceCollection.TryAddTransient(typeof(IPipeline<,>),typeof(Pipeline<,>));

        return serviceCollection;
    }
}