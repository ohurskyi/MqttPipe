using System.Reflection;
using MessagingLibrary.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MessagingLibrary.Core.Configuration.DependencyInjection;

public static class HandlerServiceCollectionExtensions
{
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
            .Where(t => !t.IsInterface && !t.IsAbstract && IsGenericMessageHandler(t))
            .ToList();

        foreach (var handlerType in implementationTypes)
        {
            serviceCollection.AddTransient(handlerType);
        }

        return serviceCollection;
    }

    private static bool IsGenericMessageHandler(Type type)
    {
        var interfaces = type.GetInterfaces();
        var match = interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessageHandler<>));
        return match;
    }
}