using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class PublishMiddleware<T> : IMessageMiddleware<T> where T : class, IMessageContract
{
    private readonly ILogger<PublishMiddleware<T>> _logger;
    private readonly ServiceFactory _serviceFactory;

    public PublishMiddleware(ILogger<PublishMiddleware<T>> logger, ServiceFactory serviceFactory)
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