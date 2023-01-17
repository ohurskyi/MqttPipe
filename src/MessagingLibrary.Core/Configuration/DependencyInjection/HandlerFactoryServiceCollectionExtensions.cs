using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MessagingLibrary.Core.Configuration.DependencyInjection;

public static class HandlerFactoryServiceCollectionExtensions
{
    public static IServiceCollection AddMessageHandlerFactory<TMessagingClientOptions>(this IServiceCollection serviceCollection) 
        where TMessagingClientOptions: IMessagingClientOptions
    {
        serviceCollection.AddRequiredServiceResolvingFactory();
        serviceCollection.TryAddSingleton<IMessageHandlerFactory<TMessagingClientOptions>, MessageHandlerFactory<TMessagingClientOptions>>();

        serviceCollection.TryAddSingleton<IMessagingContextFactory, MessagingContextFactory>();
        serviceCollection.TryAddSingleton<IMessageSerializer, MessageSerializer>();
        return serviceCollection;
    }

    private static IServiceCollection AddRequiredServiceResolvingFactory(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddTransient<ServiceFactory>(p => p.GetRequiredService);
        return serviceCollection;
    }
}