using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Processing.Executor;

public interface IMessageExecutor<TMessagingClientOptions> where TMessagingClientOptions: class, IMessagingClientOptions
{
    Task ExecuteAsync(IMessage message);
}