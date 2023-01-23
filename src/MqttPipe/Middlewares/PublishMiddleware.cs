using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class PublishMiddlewareGeneric<T> : IMessageMiddlewareGeneric<T> where T : class, IMessageContract
{
    private readonly ILogger<PublishMiddlewareGeneric<T>> _logger;
    private readonly ServiceFactory _serviceFactory;

    public PublishMiddlewareGeneric(ILogger<PublishMiddlewareGeneric<T>> logger, ServiceFactory serviceFactory)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
    }

    public async Task<HandlerResult> Handle<TMessagingClientOptions>(MessagingContext<T> context, MessageHandlerDelegate next)
        where TMessagingClientOptions : class, IMessagingClientOptions
    {
        var result = await next();
        var integrationEvents = result.ExecutionResults.OfType<IntegrationEventResult>().ToList();
        var integrationEventsCount = integrationEvents.Count;
        if (integrationEventsCount <= 0) return result;
        
        var publishTasks = new List<Task>(integrationEventsCount);
        var messageBus = _serviceFactory.GetInstance<IMessageBus<TMessagingClientOptions>>();
        
        foreach (var integrationEvent in integrationEvents)
        {
            _logger.LogDebug("Publishing integration event into topic {topicValue} of payload {type}", integrationEvent.Topic, integrationEvent.Contract.GetType().Name);
            publishTasks.Add(messageBus.Publish(integrationEvent.Contract, integrationEvent.Topic));
        }

        await Task.WhenAll(publishTasks);
        return result;
    }
}

public class PublishMiddleware<TMessagingClientOptions> : IMessageMiddleware<TMessagingClientOptions>  
    where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly ILogger<PublishMiddleware<TMessagingClientOptions>> _logger;
    private readonly IMessageBus<TMessagingClientOptions> _messageBus;

    public PublishMiddleware(ILogger<PublishMiddleware<TMessagingClientOptions>> logger, IMessageBus<TMessagingClientOptions> messageBus)
    {
        _logger = logger;
        _messageBus = messageBus;
    }

    public async Task<HandlerResult> Handle(MessagingContext context, MessageHandlerDelegate next)
    {
        var result = await next();
        var integrationEvents = result.ExecutionResults.OfType<IntegrationEventResult>().ToList();
        var publishTasks = new List<Task>(integrationEvents.Count);
        foreach (var integrationEvent in integrationEvents)
        {
            _logger.LogDebug("Publishing integration event into topic {topicValue} of payload {type}", integrationEvent.Topic, integrationEvent.Contract.GetType().Name);
            publishTasks.Add(_messageBus.Publish(integrationEvent.Contract, integrationEvent.Topic));
        }

        await Task.WhenAll(publishTasks);
        return result;
    }
}