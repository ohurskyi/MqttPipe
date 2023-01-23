using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Processing.Executor;

public abstract class MessageHandlingStrategyGenericBase
{
    public abstract Task<HandlerResult> Handle<TMessagingClientOptions>(object ctx) where TMessagingClientOptions : IMessagingClientOptions;
}

public class MessageHandlingStrategyGeneric<T> : MessageHandlingStrategyGenericBase
    where T : class, IMessageContract
{
    private readonly ServiceFactory _serviceFactory;

    public MessageHandlingStrategyGeneric(ServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }
    
    public override Task<HandlerResult> Handle<TMessagingClientOptions>(object ctx)
    {
        return HandleAsync<TMessagingClientOptions>((MessagingContext<T>)ctx);
    }

    private async Task<HandlerResult> HandleAsync<TMessagingClientOptions>(MessagingContext<T> messagingContext) where TMessagingClientOptions : IMessagingClientOptions
    {
        var factory = _serviceFactory.GetInstance<IMessageHandlerFactory<TMessagingClientOptions>>();
        var handlers = factory.GetHandlersNew<T>(messagingContext.Topic, _serviceFactory);
        var results = await HandleInner(messagingContext, handlers);
        var handlerResult = new HandlerResult();
        handlerResult.AddResults(results);
        return handlerResult;
    }

    private static async Task<List<IExecutionResult>> HandleInner(MessagingContext<T> messagingContext, IEnumerable<IMessageHandlerGeneric<T>> handlers)
    {
        var executionResults = new List<IExecutionResult>();

        foreach (var handler in handlers)
        {
            var result = await handler.HandleAsync(messagingContext);
            executionResults.Add(result);
        }

        return executionResults;
    }
}