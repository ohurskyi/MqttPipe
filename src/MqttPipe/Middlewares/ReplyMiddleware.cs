using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Extensions;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MqttPipe.Middlewares;

public class ReplyMiddleware<TMessagingClientOptions> : IMessageMiddleware<TMessagingClientOptions>  
    where TMessagingClientOptions : IMessagingClientOptions
{
    private readonly ILogger<ReplyMiddleware<TMessagingClientOptions>> _logger;
    private readonly IMessageBus<TMessagingClientOptions> _messageBus;

    public ReplyMiddleware(ILogger<ReplyMiddleware<TMessagingClientOptions>> logger, IMessageBus<TMessagingClientOptions> messageBus)
    {
        _logger = logger;
        _messageBus = messageBus;
    }

    public async Task<HandlerResult> Handle(IMessage message, MessageHandlerDelegate next)
    {
        var result = await next();
        var replyResults = result.ExecutionResults.OfType<ReplyResult>().ToList();
        var replyTasks = new List<Task>(replyResults.Count);
        foreach (var replyResult in replyResults)
        {
            _logger.LogDebug("Sending reply to topic {topicValue} of payload {type}", replyResult.ResponseContext.ReplyTopic,  replyResult.Payload.GetType().Name);
            var replyMessage = new Message { Topic = replyResult.ResponseContext.ReplyTopic, CorrelationId = replyResult.ResponseContext.CorrelationId, Payload = replyResult.Payload.MessagePayloadToJson() };
            replyTasks.Add(_messageBus.Publish(replyMessage));
        }
        await Task.WhenAll(replyTasks);
        return result;
    }
}