using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Core.Serialization;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class ReplyMiddleware<T, V>: IMessageMiddleware<T, V> 
    where T : class, IMessageContract 
    where V: class, IMessagingClientOptions
{
    private readonly ILogger<ReplyMiddleware<T, V>> _logger;
    private readonly ServiceFactory _serviceFactory;
    private readonly IMessageSerializer _messageSerializer;
    public ReplyMiddleware(ILogger<ReplyMiddleware<T, V>> logger, ServiceFactory serviceFactory, IMessageSerializer messageSerializer)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
        _messageSerializer = messageSerializer;
    }

    public async Task<HandlerResult> Handle(MessagingContext<T> context, V messagingClientOptions, MessageHandlerDelegate<T, V> next)
    {
        var result = await next(context, messagingClientOptions);
        var replyResults = result.ExecutionResults.OfType<ReplyResult>().ToList();
        var replyResultsCount = replyResults.Count;
        if (replyResultsCount <= 0) return result;
        
        var replyTasks = new List<Task>(replyResultsCount);
        var messageBus = _serviceFactory.GetInstance<IMessageBus<V>>();
        foreach (var replyResult in replyResults)
        {
            _logger.LogDebug("Sending reply to topic {topicValue} of payload {type}", replyResult.ResponseContext.ReplyTopic,  replyResult.MessageResponse.GetType().Name);
            var replyMessage = new Message { Topic = replyResult.ResponseContext.ReplyTopic, CorrelationId = replyResult.ResponseContext.CorrelationId, Payload = _messageSerializer.Serialize(replyResult.MessageResponse) };
            replyTasks.Add(messageBus.Publish(replyMessage));
        }

        await Task.WhenAll(replyTasks);
        return result;
    }
}