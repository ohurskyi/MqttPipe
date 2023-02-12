using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Resolvers;
using MessagingLibrary.Core.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MessagingLibrary.Core.Configuration.DependencyInjection;

public static class SerializationCollectionExtensions
{
    public static IServiceCollection AddMessageSerialization(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddTransient<IMessagingContextFactory, MessagingContextFactory>();
        serviceCollection.TryAddTransient<IMessageSerializer, MessageSerializer>();
        serviceCollection.TryAddSingleton<IMessageTypesResolver, MessageTypesResolver>();
        return serviceCollection;
    }
}