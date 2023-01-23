using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;

namespace MessagingLibrary.Processing.Strategy;

public class MessageHandlingStrategy<TMessagingClientOptions> : IMessageHandlingStrategy<TMessagingClientOptions>
    where TMessagingClientOptions: IMessagingClientOptions
{
    private readonly IMessageHandlerFactory<TMessagingClientOptions> _messageHandlerFactory;
    private readonly ServiceFactory _serviceFactory;

    public MessageHandlingStrategy(
        IMessageHandlerFactory<TMessagingClientOptions> messageHandlerFactory, 
        ServiceFactory serviceFactory)
    {
        _messageHandlerFactory = messageHandlerFactory;
        _serviceFactory = serviceFactory;
    }

    public async Task<HandlerResult> Handle(MessagingContext messagingContext)
    {
        return new HandlerResult();
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
        
        return executionResults;
    }
}