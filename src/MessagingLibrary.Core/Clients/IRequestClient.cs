using MessagingLibrary.Core.Configuration;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Clients;

public interface IRequestClient<TMessagingClientOptions> where TMessagingClientOptions : IMessagingClientOptions
{
    Task<TMessageResponse> SendAndWaitAsync<TMessageRequest, TMessageResponse>(string requestTopic, string responseTopic, TMessageRequest contract, TimeSpan timeout) 
        where TMessageRequest: class, IMessageRequest
        where TMessageResponse : class, IMessageResponse;
}