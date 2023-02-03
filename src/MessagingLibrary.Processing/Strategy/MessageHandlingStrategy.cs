using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Processing.Strategy;

public class MessageHandlingStrategy<T> where T : class, IMessageContract
{
    private readonly ServiceFactory _serviceFactory;

    public MessageHandlingStrategy(ServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    public async Task<HandlerResult> HandleAsync<TMessagingClientOptions>(MessagingContext<T> messagingContext) 
        where TMessagingClientOptions : class, IMessagingClientOptions
    {
        var factory = _serviceFactory.GetInstance<IMessageHandlerFactory<TMessagingClientOptions>>();
        
        var handlers = factory.GetHandlers<T>(messagingContext.Topic, _serviceFactory);

        var handlerResult = await HandleCore(messagingContext, handlers);

        return handlerResult;
    }

    private async Task<HandlerResult> HandleCore(MessagingContext<T> messagingContext, IEnumerable<IMessageHandlerGeneric<T>> handlers)
    {
        var executionResults = new List<IExecutionResult>();
        foreach (var handler in handlers)
        {
            var result = await handler.HandleAsync(messagingContext);
            executionResults.Add(result);
        }

        var handlerResult = new HandlerResult();
        handlerResult.AddResults(executionResults);
        return handlerResult;
    }
}