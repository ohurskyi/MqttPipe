using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Processing.Strategy;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingLibrary.Processing.Executor
{
    public class ScopedMessageExecutor<TMessagingClientOptions> : IMessageExecutor<TMessagingClientOptions> 
        where TMessagingClientOptions: class, IMessagingClientOptions
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScopedMessageExecutor(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(IMessage message)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var messagingContextFactory = scope.ServiceProvider.GetRequiredService<IMessagingContextFactory>();
            if (messagingContextFactory.TryGetContext(message, out var context))
            {
                var constructedType = typeof(MessageHandlingStrategy<>).MakeGenericType(context.Message.GetType());
                var handlingStrategyGenericBase = (MessageHandlingStrategyBase)scope.ServiceProvider.GetRequiredService(constructedType);
                await handlingStrategyGenericBase.Handle<TMessagingClientOptions>(context);
            }
        }
    }
}