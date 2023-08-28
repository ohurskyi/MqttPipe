using MessagingLibrary.Core.Clients;
using MessagingLibrary.Core.Messages;
using Microsoft.Extensions.DependencyInjection;
using MqttPipe.Configuration.Configuration;

namespace MqttPipe.Clients.RequestResponse;

public class RequestClient<TMessagingClientOptions> : IRequestClient<TMessagingClientOptions> where TMessagingClientOptions : class, IMqttMessagingClientOptions
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RequestClient(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<TMessageResponse> SendAndWaitAsync<TMessageRequest, TMessageResponse>(string requestTopic, string responseTopic, TMessageRequest contract, TimeSpan timeout) 
        where TMessageRequest: class, IMessageRequest
        where TMessageResponse : class, IMessageResponse
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var requester = scope.ServiceProvider.GetRequiredService<Requester<TMessagingClientOptions, TMessageRequest, TMessageResponse>>();
        var response = await requester.Request(requestTopic, responseTopic, contract, timeout);
        return response;
    }
}