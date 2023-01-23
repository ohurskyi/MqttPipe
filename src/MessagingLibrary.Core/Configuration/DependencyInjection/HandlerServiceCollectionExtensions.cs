using System.Reflection;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MessagingLibrary.Core.Configuration.DependencyInjection;

public static class HandlerServiceCollectionExtensions
{
    public static IServiceCollection AddMessageHandlerNew<TContract, THandler>(this IServiceCollection serviceCollection)
        where TContract: class, IMessageContract
        where THandler : class, IMessageHandlerGeneric<TContract>
    {
        serviceCollection.TryAddTransient<THandler>();
        return serviceCollection;
    }
    
    public static IServiceCollection AddMessageHandler<T>(this IServiceCollection serviceCollection) where T : class, IMessageHandler
    { 
        serviceCollection.TryAddTransient<T>();
        return serviceCollection;
    }

    public static IServiceCollection AddMessageHandlers(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        assemblies = assemblies.Distinct().ToArray();
        
        var implementationTypes = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(t => typeof(IMessageHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();

        foreach (var handlerType in implementationTypes)
        {
            serviceCollection.AddTransient(handlerType);
        }

        return serviceCollection;
    }
    
    public static IServiceCollection AddMessageHandlersNew(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        assemblies = assemblies.Distinct().ToArray();
        
        var implementationTypes = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(Match)
            .ToList();

        foreach (var handlerType in implementationTypes)
        {
            serviceCollection.AddTransient(handlerType);
        }

        return serviceCollection;
    }

    private static bool Match(Type type)
    {
        var interfaces = type.GetInterfaces();
        var match = interfaces.Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessageHandlerGeneric<>));
        return match;
    }
}