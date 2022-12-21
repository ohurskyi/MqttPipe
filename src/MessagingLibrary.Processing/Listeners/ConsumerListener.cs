﻿using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Definitions.Consumers;

namespace MessagingLibrary.Processing.Listeners;

public class ConsumerListener<TMessagingClientOptions> : IConsumerListener
    where TMessagingClientOptions : class, IMessagingClientOptions
{
    private readonly ITopicClient<TMessagingClientOptions> _topicClient;
    private readonly IConsumerDefinitionProvider _consumerDefinitionProvider;

    public ConsumerListener(ITopicClient<TMessagingClientOptions> topicClient, IConsumerDefinitionProvider consumerDefinitionProvider)
    {
        _topicClient = topicClient;
        _consumerDefinitionProvider = consumerDefinitionProvider;
    }

    public async Task StartListening()
    {
        var definitions = _consumerDefinitionProvider.Definitions.SelectMany(c => c.Definitions());
        await Task.WhenAll(definitions.Select(d => _topicClient.Subscribe(d)));
    }
    
    public async Task StopListening()
    {
        var definitions = _consumerDefinitionProvider.Definitions.SelectMany(c => c.Definitions());
        await Task.WhenAll(definitions.Select(d => _topicClient.Unsubscribe(d)));
    }
}