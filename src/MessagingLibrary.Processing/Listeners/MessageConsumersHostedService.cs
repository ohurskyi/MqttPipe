using Microsoft.Extensions.Hosting;

namespace MessagingLibrary.Processing.Listeners;

public class MessageConsumersHostedService : IHostedService
{
    private readonly IEnumerable<IConsumerDefinitionListenerProvider> _listenerProviders;

    public MessageConsumersHostedService(IEnumerable<IConsumerDefinitionListenerProvider> listenerProviders)
    {
        _listenerProviders = listenerProviders;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var listeners = _listenerProviders.SelectMany(p => p.Listeners);
        await Task.WhenAll(listeners.Select(listener => listener.StartListening()));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var listeners = _listenerProviders.SelectMany(p => p.Listeners);
        await Task.WhenAll(listeners.Select(listener => listener.StopListening()));
    }
}