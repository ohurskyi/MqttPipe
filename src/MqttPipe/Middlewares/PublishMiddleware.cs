using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class PublishMiddleware<T, V> : IMessageMiddleware<T, V> 
    where T : class, IMessageContract
    where V: class, IMessagingClientOptions
{
    private readonly ILogger<PublishMiddleware<T, V>> _logger;
    private readonly ServiceFactory _serviceFactory;

    public PublishMiddleware(ILogger<PublishMiddleware<T, V>> logger, ServiceFactory serviceFactory)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
    }

    public async Task<HandlerResult> Handle(MessagingContext<T> context, V messagingClientOptions, MessageHandlerDelegate<T, V> next)
    {
        var result = await next(context, messagingClientOptions);
        var integrationEvents = result.ExecutionResults.OfType<IntegrationEventResult>().ToList();
        var integrationEventsCount = integrationEvents.Count;
        if (integrationEventsCount <= 0) return result;
        
        var publishTasks = new List<Task>(integrationEventsCount);
        var messageBus = _serviceFactory.GetInstance<IMessageBus<V>>();
        
        foreach (var integrationEvent in integrationEvents)
        {
            _logger.LogDebug("Publishing integration event into topic {topicValue} of payload {type}", integrationEvent.Topic, integrationEvent.Contract.GetType().Name);
            publishTasks.Add(messageBus.Publish(integrationEvent.Contract, integrationEvent.Topic));
        }

        await Task.WhenAll(publishTasks);
        return result;
    }
}