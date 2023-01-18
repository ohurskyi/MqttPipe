using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;

namespace MessagingLibrary.Processing.Strategy;

public class MessageHandlingStrategy<TMessagingClientOptions> : IMessageHandlingStrategy<TMessagingClientOptions>
    where TMessagingClientOptions: IMessagingClientOptions
{
    private readonly IMessageHandlerFactory<TMessagingClientOptions> _messageHandlerFactory;
    private readonly IMessagingContextFactory _messagingContextFactory;
    private readonly ServiceFactory _serviceFactory;

    public MessageHandlingStrategy(
        IMessageHandlerFactory<TMessagingClientOptions> messageHandlerFactory, 
        ServiceFactory serviceFactory, 
        IMessagingContextFactory messagingContextFactory)
    {
        _messageHandlerFactory = messageHandlerFactory;
        _serviceFactory = serviceFactory;
        _messagingContextFactory = messagingContextFactory;
    }

    public async Task<HandlerResult> Handle(IMessage message)
    {
        var handlers = _messageHandlerFactory.GetHandlers(message.Topic, _serviceFactory);

        var context = _messagingContextFactory.Create(message);

        Task<HandlerResult> HandlerFunc() => HandleInner(handlers, context);

        var messageHandlerDelegate = _serviceFactory
            .GetInstances<IMessageMiddleware<TMessagingClientOptions>>()
            .Reverse()
            .Aggregate((MessageHandlerDelegate)HandlerFunc, 
                (next, pipeline) => () => pipeline.Handle(context, next));

        var handlerResult = await messageHandlerDelegate();

        return handlerResult;
    }

    private async Task<HandlerResult> HandleInner(IEnumerable<IMessageHandler> handlers, MessagingContext context)
    {
        var executionResults = await HandleCore(handlers, context);

        var handlerResult = new HandlerResult();

        foreach (var executionResult in executionResults)
        {
            handlerResult.AddResult(executionResult);
        }

        return handlerResult;
    }

    protected virtual async Task<IEnumerable<IExecutionResult>> HandleCore(IEnumerable<IMessageHandler> handlers, MessagingContext context)
    {
        var executionResults = new List<IExecutionResult>();

        foreach (var handler in handlers)
        {
            var result = await handler.Handle(context);
            executionResults.Add(result);
        }

        return executionResults;
    }
}