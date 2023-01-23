using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Factory;

public interface IMessageHandlerFactory<TMessagingClientOptions> where TMessagingClientOptions: IMessagingClientOptions
{
    int RegisterHandler(Type handlerType, string topic);
    int RegisterHandler<THandler>(string topic) where THandler : class, IMessageHandler;
    int RemoveHandler<THandler>(string topic) where THandler : class, IMessageHandler;
    int RemoveHandler(Type handlerType, string topic);
    IEnumerable<IMessageHandlerGeneric<T>> GetHandlers<T>(string topic, ServiceFactory serviceFactory)
        where T : class, IMessageContract;
}