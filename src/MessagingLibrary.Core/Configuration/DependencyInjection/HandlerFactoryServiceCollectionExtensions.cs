using MessagingLibrary.Core.Factory;
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
        return serviceCollection;
    }

    private static IServiceCollection AddRequiredServiceResolvingFactory(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddTransient<ServiceFactory>(p => p.GetRequiredService);
        return serviceCollection;
    }
}