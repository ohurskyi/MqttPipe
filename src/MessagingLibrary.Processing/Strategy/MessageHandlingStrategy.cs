using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;

namespace MessagingLibrary.Processing.Strategy;

public abstract class MessageHandlingStrategyBase
{
    public abstract Task<HandlerResult> Handle<TMessagingClientOptions>(object ctx) 
        where TMessagingClientOptions : class, IMessagingClientOptions;
}

public class MessageHandlingStrategy<T> : MessageHandlingStrategyBase
    where T : class, IMessageContract
{
    private readonly ServiceFactory _serviceFactory;

    public MessageHandlingStrategy(ServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }
    
    public override Task<HandlerResult> Handle<TMessagingClientOptions>(object ctx)
    {
        return HandleAsync<TMessagingClientOptions>((MessagingContext<T>)ctx);
    }

    private async Task<HandlerResult> HandleAsync<TMessagingClientOptions>(MessagingContext<T> messagingContext) 
        where TMessagingClientOptions : class, IMessagingClientOptions
    {
        var factory = _serviceFactory.GetInstance<IMessageHandlerFactory<TMessagingClientOptions>>();
        
        var handlers = factory.GetHandlers<T>(messagingContext.Topic, _serviceFactory);
        
        Task<HandlerResult> HandlerFunc() => HandleCore(messagingContext, handlers);

        var middlewares = _serviceFactory
            .GetInstances<IMessageMiddleware<T>>()
            .Reverse()
            .Aggregate((MessageHandlerDelegate)HandlerFunc, 
                (next, pipeline) => () => pipeline.Handle<TMessagingClientOptions>(messagingContext, next));

        var handlerResult = await middlewares();

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