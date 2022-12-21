using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Processing.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessagingLibrary.Processing.Executor
{
    public class ScopedMessageExecutor<TMessagingClientOptions> : IMessageExecutor<TMessagingClientOptions> 
        where TMessagingClientOptions: class, IMessagingClientOptions
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ScopedMessageExecutor<TMessagingClientOptions>> _logger;
        private readonly IOptionsMonitor<TMessagingClientOptions> _messagingClientOptions;

        public ScopedMessageExecutor(IServiceScopeFactory serviceScopeFactory, ILogger<ScopedMessageExecutor<TMessagingClientOptions>> logger, IOptionsMonitor<TMessagingClientOptions> messagingClientOptions)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _messagingClientOptions = messagingClientOptions;
        }

        public async Task ExecuteAsync(IMessage message)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var messagingContextFactory = scope.ServiceProvider.GetRequiredService<IMessagingContextFactory>();
            if (!messagingContextFactory.TryGetContext(message, out var context, out var serializedMessageType))
            {
                _logger.LogError("Message type : {value} could not be resolved.", serializedMessageType);
                return;
            }
            
            var messageType = context.Message.GetType();
            var constructedType = typeof(IPipeline<,>).MakeGenericType(messageType, typeof(TMessagingClientOptions));
            var pipeline = (IPipeline)scope.ServiceProvider.GetRequiredService(constructedType);
            await pipeline.Process(context, _messagingClientOptions.CurrentValue);
        }
    }
}