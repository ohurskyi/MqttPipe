using System.Collections.Concurrent;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;

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

    public int RemoveHandler(Type handlerType, string topic)
    {
        return RemoveInner(handlerType, topic);
    }

    public IEnumerable<IMessageHandler<T>> GetHandlers<T>(string topic, ServiceFactory serviceFactory) where T : class, IMessageContract
    {
        var instances = _handlersMap
            .Where(k => _topicFilterComparer.IsMatch(topic, k.Key))
            .SelectMany(k => k.Value.Keys)
            .Select(serviceFactory.GetInstance<IMessageHandler<T>>)
            .ToList();
        return instances;
    }

    private int AddInner(Type handlerType, string topic)
    {
        if (!_handlersMap.TryGetValue(topic, out var handlers))
        {
            Interlocked.Exchange(ref handlers, new ConcurrentDictionary<Type, byte>());
            handlers.TryAdd(handlerType, default);
            _handlersMap.TryAdd(topic, handlers);
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