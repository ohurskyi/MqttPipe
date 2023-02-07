using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MessagingLibrary.Processing.Executor
{
    public class ScopedMessageExecutor<TMessagingClientOptions> : IMessageExecutor<TMessagingClientOptions> 
        where TMessagingClientOptions: class, IMessagingClientOptions
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ScopedMessageExecutor<TMessagingClientOptions>> _logger;

        public ScopedMessageExecutor(IServiceScopeFactory serviceScopeFactory, ILogger<ScopedMessageExecutor<TMessagingClientOptions>> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task ExecuteAsync(IMessage message)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var messagingContextFactory = scope.ServiceProvider.GetRequiredService<IMessagingContextFactory>();
            if (messagingContextFactory.TryGetContext(message, out var context, out var serializedMessageType))
            {
                var messageType = context.Message.GetType();
                var constructedType = typeof(IPipeline<>).MakeGenericType(messageType);
                var pipeline = (IPipeline)scope.ServiceProvider.GetRequiredService(constructedType);
                await pipeline.Process<TMessagingClientOptions>(context);
            }

            _logger.LogError("Message type : {value} could not be resolved.", serializedMessageType);
        }
    }
}