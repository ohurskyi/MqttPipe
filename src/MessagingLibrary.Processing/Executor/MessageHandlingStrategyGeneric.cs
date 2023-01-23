using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace MessagingLibrary.Processing.Executor;

public abstract class MessageHandlingStrategyGenericBase
{
    public abstract Task Handle<TMessagingClientOptions>(object ctx) where TMessagingClientOptions : IMessagingClientOptions;
}

public class MessageHandlingStrategyGeneric<T> : MessageHandlingStrategyGenericBase
    where T : class, IMessageContract
{
    private readonly ServiceFactory _serviceFactory;

    public MessageHandlingStrategyGeneric(ServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }
    
    public override Task Handle<TMessagingClientOptions>(object ctx)
    {
        var factory = _serviceFactory.GetInstance<IMessageHandlerFactory<TMessagingClientOptions>>();
        return HandleAsync((MessagingContext<T>)ctx, factory);
    }

    private Task HandleAsync<TMessagingClientOptions>(MessagingContext<T> messagingContext, IMessageHandlerFactory<TMessagingClientOptions> factory) where TMessagingClientOptions : IMessagingClientOptions
    {
        var handlers = factory.GetHandlersNew<T>(messagingContext.Topic, _serviceFactory);
        foreach (var handler in handlers)
        {
            handler.HandleAsync(messagingContext);
        }
        return Task.CompletedTask;
    }

    private IEnumerable<IMessageHandlerGeneric<T>> GetHandlers(string topic, ServiceFactory serviceFactory)
    {
        var handlerTypes = new List<Type>
        {
            typeof(TestMessageHandlerGeneric), typeof(TestMessageHandlerGeneric1)
        };
        var instances = handlerTypes
            .Select(h => serviceFactory.GetInstance<IMessageHandlerGeneric<T>>(h))
            .ToList();
        return instances;
    }
}

public class ShootingInfoContractNew : IMessageContract
{
    public int LaneNumber { get; set; }
}

public class TestMessageHandlerGeneric : IMessageHandlerGeneric<ShootingInfoContractNew>
{
    public async Task<IExecutionResult> HandleAsync(MessagingContext<ShootingInfoContractNew> messagingContext)
    {
        var msg = messagingContext.Message;
        var lane = msg.LaneNumber;
        await Task.CompletedTask;
        return new SuccessfulResult();
    }
}

public class TestMessageHandlerGeneric1 : IMessageHandlerGeneric<ShootingInfoContractNew>
{
    public async Task<IExecutionResult> HandleAsync(MessagingContext<ShootingInfoContractNew> messagingContext)
    {
        var msg = messagingContext.Message;
        await Task.CompletedTask;
        return new SuccessfulResult();
    }
}