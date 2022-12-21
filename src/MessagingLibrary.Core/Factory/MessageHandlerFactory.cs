using System.Collections.Concurrent;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Handlers;

namespace MessagingLibrary.Core.Factory;

public class MessageHandlerFactory<TMessagingClientOptions> : IMessageHandlerFactory<TMessagingClientOptions>
    where TMessagingClientOptions: IMessagingClientOptions
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Type, byte>> _handlersMap = new();
    
    private readonly ITopicFilterComparer _topicFilterComparer;

    public MessageHandlerFactory(ITopicFilterComparer topicFilterComparer)
    {
        _topicFilterComparer = topicFilterComparer;
    }
    
    public int RegisterHandler(Type handlerType, string topic)
    {
        return AddInner(handlerType, topic);
    }

    public int RegisterHandler<THandler>(string topic) where THandler : class, IMessageHandler
    {
        return AddInner(typeof(THandler), topic);
    }

    public int RemoveHandler<THandler>(string topic) where THandler : class, IMessageHandler
    {
        return RemoveInner(typeof(THandler), topic);
    }

    public int RemoveHandler(Type handlerType, string topic)
    {
        return RemoveInner(handlerType, topic);
    }

    public IEnumerable<IMessageHandler> GetHandlers(string topic, ServiceFactory serviceFactory)
    {
        var instances = _handlersMap
            .Where(k => _topicFilterComparer.IsMatch(topic, k.Key))
            .SelectMany(k => k.Value.Keys)
            .Select(serviceFactory.GetInstance<IMessageHandler>);

        return instances;
    }

    private int AddInner(Type handlerType, string topic)
    {
        if (!_handlersMap.TryGetValue(topic, out var handlers))
        {
            _handlersMap.TryAdd(topic, new ConcurrentDictionary<Type, byte>(new[] { new KeyValuePair<Type, byte>(handlerType, default) }));
            return 1;
        }

        if (handlers.ContainsKey(handlerType))
        {
            return handlers.Count;
        }

        handlers.TryAdd(handlerType, default);

        return handlers.Count;
    }
    
    private int RemoveInner(Type handlerType, string topic)
    {
        if (!_handlersMap.TryGetValue(topic, out var handlers))
        {
            return -1;
        }

        handlers.TryRemove(handlerType, out _);

        if (handlers.IsEmpty)
        {
            _handlersMap.TryRemove(topic, out _);
        }

        return handlers.Count;
    }
}