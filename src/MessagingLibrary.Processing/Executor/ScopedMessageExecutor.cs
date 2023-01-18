using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Processing.Strategy;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingLibrary.Processing.Executor
{
    public class ScopedMessageExecutor<TMessagingClientOptions> : IMessageExecutor<TMessagingClientOptions> 
        where TMessagingClientOptions: IMessagingClientOptions
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScopedMessageExecutor(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync(IMessage message)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var messageHandlingStrategy = scope.ServiceProvider.GetRequiredService<IMessageHandlingStrategy<TMessagingClientOptions>>();
            var messagingContextFactory = scope.ServiceProvider.GetRequiredService<IMessagingContextFactory>();
            var context = messagingContextFactory.Create(message);
            await messageHandlingStrategy.Handle(context);
        }
    }
}