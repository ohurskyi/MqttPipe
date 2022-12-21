using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

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

    public async Task<HandlerResult> Handle(IMessage message, MessageHandlerDelegate next)
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