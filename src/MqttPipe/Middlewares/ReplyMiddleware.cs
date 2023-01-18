using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Extensions;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Core.Serialization;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class ReplyMiddleware<TMessagingClientOptions> : IMessageMiddleware<TMessagingClientOptions>  
    where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly ILogger<ReplyMiddleware<TMessagingClientOptions>> _logger;
    private readonly IMessageBus<TMessagingClientOptions> _messageBus;
    private readonly IMessageSerializer _messageSerializer;

    public ReplyMiddleware(ILogger<ReplyMiddleware<TMessagingClientOptions>> logger, IMessageBus<TMessagingClientOptions> messageBus, IMessageSerializer messageSerializer)
    {
        _logger = logger;
        _messageBus = messageBus;
        _messageSerializer = messageSerializer;
    }

    public async Task<HandlerResult> Handle(MessagingContext context, MessageHandlerDelegate next)
    {
        var result = await next();
        var replyResults = result.ExecutionResults.OfType<ReplyResult>().ToList();
        var replyTasks = new List<Task>(replyResults.Count);
        foreach (var replyResult in replyResults)
        {
            _logger.LogDebug("Sending reply to topic {topicValue} of payload {type}", replyResult.ResponseContext.ReplyTopic,  replyResult.Payload.GetType().Name);
            var replyMessage = new Message { Topic = replyResult.ResponseContext.ReplyTopic, CorrelationId = replyResult.ResponseContext.CorrelationId, Payload = _messageSerializer.Serialize(replyResult.Payload) };
            replyTasks.Add(_messageBus.Publish(replyMessage));
        }

        await Task.WhenAll(replyTasks);
        return result;
    }
}